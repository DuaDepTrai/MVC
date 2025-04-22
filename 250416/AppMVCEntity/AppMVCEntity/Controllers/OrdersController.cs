using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AppMVCEntity.Controllers
{
    public class OrdersController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.OrderByDescending(o=>o.OrderID).ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            var order = db.Orders
            .Include(o => o.Order_Details.Select(od => od.Product)) // Load thêm Product nếu muốn hiển thị tên
            .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var Cus = db.Customers.Select(s => new { CustomerID = s.CustomerID, CompanyName = s.CompanyName }).ToList();
            ViewBag.CustomerID = new SelectList(Cus, "CustomerID", "CompanyName");

            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName");

            var Shp = db.Shippers.Select(s => new { ShipperID = s.ShipperID, CompanyName = s.CompanyName }).ToList();
            ViewBag.ShipperID = new SelectList(Shp, "ShipperID", "CompanyName");

            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        public ActionResult Create(Order obj)
        {
            if (!ModelState.IsValid) { 
                var Cus = db.Customers.Select(s => new { CustomerID = s.CustomerID, CompanyName = s.CompanyName }).ToList();
                ViewBag.CustomerID = new SelectList(Cus, "CustomerID", "CompanyName", obj.CustomerID);

                var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
                ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", obj.EmployeeID);

                var Shp = db.Shippers.Select(s => new { ShipperID = s.ShipperID, CompanyName = s.CompanyName }).ToList();
                ViewBag.ShipperID = new SelectList(Shp, "ShipperID", "CompanyName", obj.ShipVia);

                return View(obj);
            }
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Orders.Add(obj);
                    db.SaveChanges();

                    // Lấy OrderID vừa tạo
                    int newOrderId = obj.OrderID;

                    // Lấy danh sách OrderDetails từ Session
                    var orderDetails = Session["listOrders"] as List<Order_Detail>;
                    if (orderDetails != null)
                    {
                        foreach (var item in orderDetails)
                        {
                            item.OrderID = newOrderId; // Gán OrderID mới vào từng OrderDetail
                            db.Order_Details.Add(item);
                        }
                        db.SaveChanges();
                        Session["listOrders"] = null; // Xoá session sau khi lưu xong
                    }

                    return RedirectToAction("Index");

                }
                return View(obj);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                var Cus = db.Customers.Select(s => new { CustomerID = s.CustomerID, CompanyName = s.CompanyName }).ToList();
                ViewBag.CustomerID = new SelectList(Cus, "CustomerID", "CompanyName", obj.CustomerID);

                var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
                ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", obj.EmployeeID);

                var Shp = db.Shippers.Select(s => new { ShipperID = s.ShipperID, CompanyName = s.CompanyName }).ToList();
                ViewBag.ShipperID = new SelectList(Shp, "ShipperID", "CompanyName", obj.ShipVia);

                return View(obj);
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            var order = db.Orders.Include(o => o.Order_Details) // Ensure Order_Details are loaded
                                 .FirstOrDefault(o => o.OrderID == id);

            var Cus = db.Customers.Select(s => new { CustomerID = s.CustomerID, CompanyName = s.CompanyName }).ToList();
            ViewBag.CustomerID = new SelectList(Cus, "CustomerID", "CompanyName", order.CustomerID);

            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", order.EmployeeID);

            var Shp = db.Shippers.Select(s => new { ShipperID = s.ShipperID, CompanyName = s.CompanyName }).ToList();
            ViewBag.ShipperID = new SelectList(Shp, "ShipperID", "CompanyName", order.ShipVia);

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Order obj)
        {
            var Cus = db.Customers.Select(s => new { CustomerID = s.CustomerID, CompanyName = s.CompanyName }).ToList();
            ViewBag.CustomerID = new SelectList(Cus, "CustomerID", "CompanyName", obj.CustomerID);

            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", obj.EmployeeID);

            var Shp = db.Shippers.Select(s => new { ShipperID = s.ShipperID, CompanyName = s.CompanyName }).ToList();
            ViewBag.ShipperID = new SelectList(Shp, "ShipperID", "CompanyName", obj.ShipVia);

            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified; db.SaveChanges();
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message; 
                return View();
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            Order order = db.Orders.Find(id);
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Order obj)
        {
            try
            {
                // TODO: Add delete logic here
                Order order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
