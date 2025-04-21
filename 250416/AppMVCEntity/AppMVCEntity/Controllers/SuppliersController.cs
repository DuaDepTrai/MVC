using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class SuppliersController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Shippers
        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int id)
        {
            Supplier supp = db.Suppliers.Find(id);
            return View(supp);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        public ActionResult Create(Supplier obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Suppliers.Add(obj);
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

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int id)
        {
            Supplier supp = db.Suppliers.Find(id);

            return View(supp);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Supplier obj)
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

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int id)
        {
            Supplier supp = db.Suppliers.Find(id);

            return View(supp);
        }

        // POST: Suppliers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Supplier obj)
        {
            try
            {
                // TODO: Add delete logic here
                Supplier supp = db.Suppliers.Find(id);
                db.Suppliers.Remove(supp);
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
