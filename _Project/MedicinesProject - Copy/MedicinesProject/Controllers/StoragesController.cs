using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicinesProject;
using MedicinesProject.Models.ViewModels;

namespace MedicinesProject.Controllers
{
    public class StoragesController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: Storages
        public ActionResult Index(string searchStr, string activeIngredientFilter, string supplierFilter, int page = 1)
        {
            //ViewBag
            ViewBag.CurrentSearch = searchStr;
            ViewBag.ActiveIngredientFilter = activeIngredientFilter?.Trim();
            ViewBag.SupplierFilter = supplierFilter?.Trim();

            var storages = db.Storages.Include(s => s.Medicines).Include(s => s.Suppliers);

            // Populate dropdown filters
            ViewBag.ActiveIngredients = db.Medicines.Select(m => m.ActiveIngredient.Trim()).Distinct().OrderBy(x => x).ToList();
            ViewBag.Suppliers = db.Suppliers.Select(c => c.SupplierName.Trim()).Distinct().OrderBy(x => x).ToList();
            
            // Apply search & filters
            if (!string.IsNullOrEmpty(searchStr))
            {
                storages = storages.Where(m => m.ReceiptCode.Contains(searchStr) || 
                                        m.BatchNumber.Contains(searchStr) || 
                                        m.Medicines.TradeName.Contains(searchStr) || 
                                        m.Medicines.ActiveIngredient.Contains(searchStr) || 
                                        m.Suppliers.SupplierName.Contains(searchStr));
            }
            if (!string.IsNullOrEmpty(activeIngredientFilter))
            {
                storages = storages.Where(m => m.Medicines.ActiveIngredient == activeIngredientFilter);
            }
            if (!string.IsNullOrEmpty(supplierFilter))
            {
                storages = storages.Where(m => m.Suppliers.SupplierName == supplierFilter);
            }

            //Pagination Controller
            int sizePerPage = 13;
            int totalItems = storages.Count();
            var pagedStorages = storages.OrderByDescending(m => m.MedicineID)
                                        .Skip((page - 1) * sizePerPage)
                                        .Take(sizePerPage)
                                        .ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / sizePerPage);

            //Return
            return View(pagedStorages);
        }

        // GET: Storages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storages storages = db.Storages.Find(id);
            if (storages == null)
            {
                return HttpNotFound();
            }
            return View(storages);
        }

        // GET: Storages/Create
        public ActionResult Create()
        {
            // Generate ReceiptCode
            string todayPrefix = DateTime.Now.ToString("yyMMdd");
            int countToday = db.Storages
                               .Count(s => s.ReceiptCode.StartsWith(todayPrefix));
            string serial = (countToday + 1).ToString("D3");
            string receiptCode = todayPrefix + serial;

            var storage = new Storages
            {
                ReceiptCode = receiptCode,
                ImportDate = DateTime.Now // có thể set sẵn
            };


            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "TradeName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName");
            return View(storage);
        }

        // POST: Storages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StorageID,ReceiptCode,BatchNumber,MedicineID,ManufactureDate,ExpiryDate,Quantity,ImportPrice,SupplierID,ImportDate")] Storages storages)
        {
            if (ModelState.IsValid)
            {
                db.Storages.Add(storages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "TradeName", storages.MedicineID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", storages.SupplierID);
            return View(storages);
        }

        // GET: Storages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storages storages = db.Storages.Find(id);
            if (storages == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "TradeName", storages.MedicineID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", storages.SupplierID);
            return View(storages);
        }

        // POST: Storages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StorageID,ReceiptCode,BatchNumber,MedicineID,ManufactureDate,ExpiryDate,Quantity,ImportPrice,SupplierID,ImportDate")] Storages storages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicineID = new SelectList(db.Medicines, "MedicineID", "TradeName", storages.MedicineID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", storages.SupplierID);
            return View(storages);
        }

        // GET: Storages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Storages storages = db.Storages.Find(id);
            if (storages == null)
            {
                return HttpNotFound();
            }
            return View(storages);
        }

        // POST: Storages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Storages storages = db.Storages.Find(id);
            db.Storages.Remove(storages);
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
