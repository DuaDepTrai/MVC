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

        // GET: OrderDetails/Details?OrderID=1&ProductID=5
        public ActionResult Details(int OrderID, int ProductID, string position)
        {
            ViewBag.Position = position;

            var listOrders = Session["listOrders"] as List<OrderDetails>;
            if (listOrders == null)
            {
                return HttpNotFound("Không tìm thấy session đơn hàng.");
            }

            var detail = listOrders.FirstOrDefault(o => o.OrderID == OrderID && o.ProductID == ProductID);
            if (detail == null)
            {
                return HttpNotFound("Không tìm thấy chi tiết đơn hàng.");
            }

            LoadProducts();
            return View(detail);
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
                
                return RedirectToAction("Create", "Orders");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderDetails/Edit
        public ActionResult Edit(int OrderID, int ProductID, string position)
        {
            ViewBag.Position = position;

            var listOrders = Session["listOrders"] as List<OrderDetails>;
            if (listOrders == null)
            {
                return HttpNotFound("Không tìm thấy session đơn hàng.");
            }

            var detail = listOrders.FirstOrDefault(o => o.OrderID == OrderID && o.ProductID == ProductID);
            if (detail == null)
            {
                return HttpNotFound("Không tìm thấy chi tiết đơn hàng.");
            }

            LoadProducts();
            return View(detail);
        }


        // POST: OrderDetails/Edit
        [HttpPost]
        public ActionResult Edit(OrderDetails obj, string position)
        {
            try
            {
                var listOrders = Session["listOrders"] as List<OrderDetails>;
                if (listOrders == null)
                {
                    return HttpNotFound("Không tìm thấy session đơn hàng.");
                }

                var existingDetail = listOrders.FirstOrDefault(o => o.OrderID == obj.OrderID && o.ProductID == obj.ProductID);

                if (existingDetail != null)
                {
                    existingDetail.Quantity = obj.Quantity;
                    existingDetail.UnitPrice = obj.UnitPrice;
                    existingDetail.Discount = obj.Discount;
                    existingDetail.ProductName = obj.ProductName;
                    existingDetail.ProductID = obj.ProductID;  // Đảm bảo ProductID được cập nhật
                }

                Session["listOrders"] = listOrders;

                // ➤ Quay lại đúng màn hình
                if (position == "Edit")
                {
                    return RedirectToAction("Edit", "Orders", new { id = obj.OrderID });
                }

                return RedirectToAction("Create", "Orders");
            }
            catch
            {
                LoadProducts();
                return View(obj);
            }
        }


        // GET: OrderDetails/Delete
        public ActionResult Delete(int OrderID, int ProductID)
        {
            List<OrderDetails> listOrders = Session["listOrders"] as List<OrderDetails>;

            if (listOrders == null)
            {
                return HttpNotFound("Không tìm thấy session đơn hàng.");
            }

            var orderDetail = listOrders.FirstOrDefault(o => o.OrderID == OrderID && o.ProductID == ProductID);

            if (orderDetail == null)
            {
                return HttpNotFound("Không tìm thấy sản phẩm trong đơn hàng.");
            }

            return View(orderDetail);
        }


        // POST: OrderDetails/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int OrderID, int ProductID)
        {
            List<OrderDetails> listOrders = Session["listOrders"] as List<OrderDetails>;

            if (listOrders != null)
            {
                var itemToRemove = listOrders.FirstOrDefault(o => o.OrderID == OrderID && o.ProductID == ProductID);
                if (itemToRemove != null)
                {
                    listOrders.Remove(itemToRemove);
                    Session["listOrders"] = listOrders;
                }
            }

            return RedirectToAction("Create", "Orders");
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
