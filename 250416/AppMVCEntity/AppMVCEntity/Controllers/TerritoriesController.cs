using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AppMVCEntity.Controllers
{
    public class TerritoriesController : Controller
    {
        private NorthwindEntities1 db = new NorthwindEntities1();

        // GET: Territories
        public ActionResult Index(int page = 1)
        {
            int sizePerPage = 10;
            int totalItems = db.Territories.Count();
            var ter = db.Territories.Include(e => e.Region)
                                        .OrderBy(e => e.TerritoryID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            return View(ter.ToList());
        }

        // GET: Territories/Details/5
        public ActionResult Details(string id)
        {
            Territory ter = db.Territories.Find(id);
            return View(ter);
        }

        // GET: Territories/Create
        public ActionResult Create()
        {
            var reg = db.Regions.Select(s => new {RegionID = s.RegionID, RegionDescription = s.RegionDescription}).ToList();    
            ViewBag.Regions = new SelectList(reg, "RegionID", "RegionDescription");

            return View();
        }

        // POST: Territories/Create
        [HttpPost]
        public ActionResult Create(Territory obj)
        {
            var reg = db.Regions.Select(s => new { RegionID = s.RegionID, RegionDescription = s.RegionDescription }).ToList();
            ViewBag.Regions = new SelectList(reg, "RegionID", "RegionDescription", obj.RegionID);

            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.Territories.Add(obj);
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

        // GET: Territories/Edit/5
        public ActionResult Edit(string id)
        {
            Territory ter = db.Territories.Find(id);
            var reg = db.Regions.Select(s => new { RegionID = s.RegionID, RegionDescription = s.RegionDescription }).ToList();
            ViewBag.Regions = new SelectList(reg, "RegionID", "RegionDescription", ter.RegionID);

            return View(ter);
        }

        // POST: Territories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Territory obj)
        {
            var reg = db.Regions.Select(s => new { RegionID = s.RegionID, RegionDescription = s.RegionDescription }).ToList();
            ViewBag.Regions = new SelectList(reg, "RegionID", "RegionDescription", obj.RegionID);

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

        // GET: Territories/Delete/5
        public ActionResult Delete(string id)
        {
            Territory ter = db.Territories.Find(id);

            return View(ter);
        }

        // POST: Territories/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Territory obj)
        {
            try
            {
                // TODO: Add delete logic here
                Territory ter = db.Territories.Find(id);
                db.Territories.Remove(ter);
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
