using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;
using System.Configuration;

namespace Homework.Controllers
{
    public class OrderDetailsController : Controller
    {
        string strcnn = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        // GET: OrderDetails
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            LoadProducts();
            return View();
        }

        // POST: OrderDetails/Create
        [HttpPost]
        public ActionResult Create(OrderDetails obj)
        {
            try
            {
                List<OrderDetails> listOrders = new List<OrderDetails>();

                if (Session["listOrders"] != null)
                {
                    listOrders = (List<Homework.Models.OrderDetails>)Session["listOrders"];
                }

                listOrders.Add(obj);
                Session["listOrders"] = listOrders;
                // TODO: Add insert logic here

                return RedirectToAction("Create", "Orders");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderDetails/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderDetails/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        private void LoadProducts()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT ProductID, ProductName FROM Products";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Products> products = new List<Products>();
            foreach (DataRow item in dt.Rows)
            {
                Products product = new Products();
                product.ProductID = int.Parse(item["productID"].ToString());
                product.ProductName = item["productName"].ToString();
                products.Add(product);
            }
            ViewBag.Products = products;
        }
    }
}
