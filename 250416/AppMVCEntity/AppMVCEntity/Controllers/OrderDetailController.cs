using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class OrderDetailController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: OrderDetail
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderDetail/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderDetail/Create
        public ActionResult Create()
        {
            var Product = db.Products.Select(s => new { ProductID = s.ProductID, ProductName = s.ProductName }).ToList();
            ViewBag.ProductID = new SelectList(Product, "ProductID", "ProductName");

            return View();
        }

        // POST: OrderDetail/Create
        [HttpPost]
        public ActionResult Create(Order_Detail obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj.OrderID == 0)
                {
                    // Trường hợp 1: add product khi create order, lưu vào session
                    List<Order_Detail> listOrders = new List<Order_Detail>();

                    if (Session["listOrders"] != null)
                    {
                        listOrders = (List<Order_Detail>)Session["listOrders"];
                    }

                    var product = db.Products.Find(obj.ProductID);
                    obj.Product = product;  // Gán sản phẩm vào order detail

                    listOrders.Add(obj);
                    Session["listOrders"] = listOrders;

                    return RedirectToAction("Create", "Orders");
                }
                else
                {
                    var product = db.Products.Find(obj.ProductID);
                    obj.Product = product;  // Gán sản phẩm vào order detail

                    // Trường hợp 2: add product khi edit order, lưu trực tiếp vào DB
                    db.Order_Details.Add(obj);
                    db.SaveChanges();

                    return RedirectToAction("Edit", "Orders", new { id = obj.OrderID });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderDetail/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderDetail/Edit/5
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

        // GET: OrderDetail/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderDetail/Delete/5
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
