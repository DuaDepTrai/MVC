using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppMVCEntity.Controllers
{
    public class ProductController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            var Supp = db.Suppliers.Select(s => new { SupplierID = s.SupplierID, CompanyName = s.CompanyName }).ToList();
            ViewBag.SupplierID = new SelectList(Supp, "SupplierID", "CompanyName");

            var Cat = db.Categories.Select(s => new { CategoryID = s.CategoryID, CategoryName = s.CategoryName }).ToList();
            ViewBag.CategoryID = new SelectList(Cat, "CategoryID", "CategoryName");

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product obj)
        {
            var Supp = db.Suppliers.Select(s => new { SupplierID = s.SupplierID, CompanyName = s.CompanyName }).ToList();
            ViewBag.SupplierID = new SelectList(Supp, "SupplierID", "CompanyName", obj.SupplierID);

            var Cat = db.Categories.Select(s => new { CategoryID = s.CategoryID, CategoryName = s.CategoryName }).ToList();
            ViewBag.CategoryID = new SelectList(Cat, "CategoryID", "CategoryName", obj.CategoryID);

            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Products.Add(obj);
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

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);

            var Supp = db.Suppliers.Select(s => new { SupplierID = s.SupplierID, CompanyName = s.CompanyName }).ToList();
            ViewBag.SupplierID = new SelectList(Supp, "SupplierID", "CompanyName", product.SupplierID);

            var Cat = db.Categories.Select(s => new { CategoryID = s.CategoryID, CategoryName = s.CategoryName }).ToList();
            ViewBag.CategoryID = new SelectList(Cat, "CategoryID", "CategoryName", product.CategoryID);

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Product obj)
        {
            var Supp = db.Suppliers.Select(s => new { SupplierID = s.SupplierID, CompanyName = s.CompanyName }).ToList();
            ViewBag.SupplierID = new SelectList(Supp, "SupplierID", "CompanyName", obj.SupplierID);

            var Cat = db.Categories.Select(s => new { CategoryID = s.CategoryID, CategoryName = s.CategoryName }).ToList();
            ViewBag.CategoryID = new SelectList(Cat, "CategoryID", "CategoryName", obj.CategoryID);

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

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Product obj)
        {
            try
            {
                // TODO: Add delete logic here
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
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
