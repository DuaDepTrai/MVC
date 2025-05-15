using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicinesProject;

namespace MedicinesProject.Controllers
{
    public class ManufacturersController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: Manufacturers
        public ActionResult Index(string searchStr, string countryFilter, int page = 1)
        {
            //ViewBag
            ViewBag.CurrentSearch = searchStr;
            ViewBag.CountryFilter = countryFilter?.Trim();

            var manus = db.Manufacturers.AsQueryable();

            // Populate dropdown filters
            ViewBag.Countries = db.Manufacturers.Select(m => m.Country.Trim()).Distinct().OrderBy(x => x).ToList();

            // Apply search & filters
            if (!string.IsNullOrEmpty(searchStr))
            {
                manus = manus.Where(m => m.Name.Contains(searchStr) ||
                                        m.Phone.Contains(searchStr) ||
                                        m.Email.Contains(searchStr));
            }
            if (!string.IsNullOrEmpty(countryFilter))
            {
                manus = manus.Where(m => m.Country == countryFilter);
            }

            //Pagination Controller
            int sizePerPage = 13;
            int totalItems = manus.Count();
            var pagedManus = manus.OrderByDescending(m => m.ManufacturerID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            //Return
            return View(pagedManus);
        }

        // GET: Manufacturers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturers manufacturers = db.Manufacturers.Find(id);
            if (manufacturers == null)
            {
                return HttpNotFound();
            }
            return View(manufacturers);
        }

        // GET: Manufacturers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManufacturerID,Name,Address,Phone,Email,Country")] Manufacturers manufacturers)
        {
            if (ModelState.IsValid)
            {
                if (db.Manufacturers.Any(m => m.Name == manufacturers.Name))
                {
                    ModelState.AddModelError("Name", "Manufacturer Name is already used");
                    return View(manufacturers);
                }
                if (db.Manufacturers.Any(m => m.Phone == manufacturers.Phone))
                {
                    ModelState.AddModelError("Phone", "Phone Number is already used");
                    return View(manufacturers);
                }
                if (db.Manufacturers.Any(m => m.Email == manufacturers.Email))
                {
                    ModelState.AddModelError("Email", "Email is already used");
                    return View(manufacturers);
                }

                db.Manufacturers.Add(manufacturers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manufacturers);
        }

        // GET: Manufacturers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturers manufacturers = db.Manufacturers.Find(id);
            if (manufacturers == null)
            {
                return HttpNotFound();
            }
            return View(manufacturers);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManufacturerID,Name,Address,Phone,Email,Country")] Manufacturers manufacturers)
        {
            if (ModelState.IsValid)
            {
                if (db.Manufacturers.Any(m => m.Name == manufacturers.Name && m.ManufacturerID != manufacturers.ManufacturerID))
                {
                    ModelState.AddModelError("Name", "Manufacturer Name is already used");
                    return View(manufacturers);
                }
                if (db.Manufacturers.Any(m => m.Phone == manufacturers.Phone && m.ManufacturerID != manufacturers.ManufacturerID))
                {
                    ModelState.AddModelError("Phone", "Phone Number is already used");
                    return View(manufacturers);
                }
                if (db.Manufacturers.Any(m => m.Email == manufacturers.Email && m.ManufacturerID != manufacturers.ManufacturerID))
                {
                    ModelState.AddModelError("Email", "Email is already used");
                    return View(manufacturers);
                }

                db.Entry(manufacturers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacturers);
        }

        // GET: Manufacturers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacturers manufacturers = db.Manufacturers.Find(id);
            if (manufacturers == null)
            {
                return HttpNotFound();
            }
            return View(manufacturers);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manufacturers manufacturers = db.Manufacturers.Find(id);
            if (db.Medicines.Any(s => s.ManufacturerID == manufacturers.ManufacturerID))
            {
                ModelState.AddModelError("", "Unable to delete this manufacturer because it is currently used in medicines data!");
                return View(manufacturers);
            }
            db.Manufacturers.Remove(manufacturers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
