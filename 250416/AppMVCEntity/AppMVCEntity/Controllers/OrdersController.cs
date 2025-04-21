using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class OrdersController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            Order order = db.Orders.Find(id);
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
            var Cus = db.Customers.Select(s => new { CustomerID = s.CustomerID, CompanyName = s.CompanyName }).ToList();
            ViewBag.CustomerID = new SelectList(Cus, "CustomerID", "CompanyName", obj.CustomerID);

            var Emp = db.Employees.Select(s => new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }).ToList();
            ViewBag.EmployeeID = new SelectList(Emp, "EmployeeID", "FullName", obj.EmployeeID);

            var Shp = db.Shippers.Select(s => new { ShipperID = s.ShipperID, CompanyName = s.CompanyName }).ToList();
            ViewBag.ShipperID = new SelectList(Shp, "ShipperID", "CompanyName", obj.ShipVia);

            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Orders.Add(obj);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View(obj);
            }
            catch
            {
                return View(obj);
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            Order order = db.Orders.Find(id);

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
            catch
            {
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
