using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class ShippersController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Shippers
        public ActionResult Index()
        {
            return View(db.Shippers.ToList());
        }

        // GET: Shippers/Details/5
        public ActionResult Details(int id)
        {
            Shipper shp = db.Shippers.Find(id);
            return View(shp);
        }

        // GET: Shippers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shippers/Create
        [HttpPost]
        public ActionResult Create(Shipper obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Shippers.Add(obj);
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

        // GET: Shippers/Edit/5
        public ActionResult Edit(int id)
        {
            Shipper shp = db.Shippers.Find(id);

            return View(shp);
        }

        // POST: Shippers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Shipper obj)
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

        // GET: Shippers/Delete/5
        public ActionResult Delete(int id)
        {
            Shipper shp = db.Shippers.Find(id);

            return View(shp);
        }

        // POST: Shippers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Shipper obj)
        {
            try
            {
                // TODO: Add delete logic here
                Shipper shp = db.Shippers.Find(id);
                db.Shippers.Remove(shp);
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
