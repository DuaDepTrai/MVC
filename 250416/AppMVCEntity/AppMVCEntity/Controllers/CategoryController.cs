using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class CategoryController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Category
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            Category cat = db.Categories.Find(id);
            return View(cat);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Categories.Add(obj);
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

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            Category cat = db.Categories.Find(id);

            return View(cat);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Category obj)
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

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            Category cat = db.Categories.Find(id);

            return View(cat);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Category obj)
        {
            try
            {
                // TODO: Add delete logic here
                Category cat = db.Categories.Find(id);
                db.Categories.Remove(cat);
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
