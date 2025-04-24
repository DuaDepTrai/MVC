using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class CustomersController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Customers
        public ActionResult Index(int page = 1)
        {
            int sizePerPage = 6;
            int totalItems = db.Customers.Count();
            var cus = db.Customers.OrderBy(e => e.CustomerID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            return View(cus.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            Customer cus= db.Customers.Find(id);
            return View(cus);
        }


        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Customers.Add(obj);
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

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            Customer cus= db.Customers.Find(id);
            return View(cus);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer obj)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            Customer cus= db.Customers.Find(id);

            return View(cus);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Customer obj)
        {
            try
            {
                // TODO: Add delete logic here
                Customer cus= db.Customers.Find(id);
                db.Customers.Remove(cus);
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
