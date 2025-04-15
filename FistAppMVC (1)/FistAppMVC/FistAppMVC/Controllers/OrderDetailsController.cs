using FistAppMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace FistAppMVC.Controllers
{
    public class OrderDetailsController : Controller
    {
        string strcnn = ConfigurationManager.ConnectionStrings["Chuoiketnoi"].ConnectionString;
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
            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT ProductID,ProductName FROM Products";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Products> listemp = new List<Products>();
            foreach (DataRow dr in dt.Rows)
            {
                Products pro = new Products();
                pro.ProductID = int.Parse(dr["ProductID"].ToString());
                pro.ProductName = dr["ProductName"].ToString();
                listemp.Add(pro);
            }
            ViewBag.Products = listemp;


            return View();
        }

        // POST: OrderDetails/Create
        [HttpPost]
        public ActionResult Create(OrderDetails obj)
        {
            try
            {
                
                List<OrderDetails> listorders = new List<OrderDetails>();

                if (Session["listorders"] != null)
                {


                    listorders = (List<FistAppMVC.Models.OrderDetails>)Session["listorders"];

                }

                listorders.Add(obj);

                Session["listorders"] = listorders;


                // TODO: Add insert logic here
                if (Session["ViTri"] == "Create")
                {
                    return RedirectToAction("Create", "Orders");
                }
                else 
                {
                    return RedirectToAction("Edit", "Orders");
                }
                
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
    }
}
