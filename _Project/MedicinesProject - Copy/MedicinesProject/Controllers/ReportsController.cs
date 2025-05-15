using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicinesProject.Models;
using MedicinesProject.Models.ViewModels;

namespace MedicinesProject.Controllers
{
    public class ReportsController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: Reports
        public ActionResult Index(string searchStr, int? months, int inventoryPage = 1, int expiredPage = 1, int nearExpiryPage = 1, string viewName = "")
        {
            var now = DateTime.Now;
            int nearExpiryMonths = months ?? 3; // Default is 3 months
            var nearExpiryThreshold = now.AddMonths(nearExpiryMonths);

            // ViewBag for search string
            ViewBag.CurrentSearch = searchStr;

            // Pagination Settings
            int sizePerPage = 13; 
            
            // Define how many items per page
            var queryInventory = db.Storages
                .Where(s => s.Quantity > 0)
                .Include(s => s.Medicines)
                .Include(s => s.Suppliers);

            var queryExpired = db.Storages
                .Where(s => s.ExpiryDate < now)
                .Include(s => s.Medicines);

            var queryNearExpiry = db.Storages
                .Where(s => s.ExpiryDate >= now && s.ExpiryDate <= nearExpiryThreshold)
                .Include(s => s.Medicines);

            // Apply Search Filters
            if (!string.IsNullOrEmpty(searchStr))
            {
                queryInventory = queryInventory.Where(s => 
                    s.Medicines.TradeName.Contains(searchStr) ||
                    s.Medicines.ActiveIngredient.Contains(searchStr) ||
                    s.BatchNumber.Contains(searchStr) ||
                    s.Medicines.Manufacturers.Name.Contains(searchStr) ||
                    s.Suppliers.SupplierName.Contains(searchStr));

                queryExpired = queryExpired.Where(s => 
                    s.ReceiptCode.Contains(searchStr) ||
                    s.Medicines.TradeName.Contains(searchStr) ||
                    s.Medicines.ActiveIngredient.Contains(searchStr) ||
                    s.BatchNumber.Contains(searchStr));

                queryNearExpiry = queryNearExpiry.Where(s =>
                    s.ReceiptCode.Contains(searchStr) ||
                    s.Medicines.TradeName.Contains(searchStr) ||
                    s.Medicines.ActiveIngredient.Contains(searchStr) ||
                    s.BatchNumber.Contains(searchStr));
            }

            // Pagination Controller
            var totalItemsInventory = queryInventory.Count();
            var pagedInventory = queryInventory.OrderBy(s => s.BatchNumber)
                .Skip((inventoryPage - 1) * sizePerPage)
                .Take(sizePerPage)
                .Select(s => new StorageReportItem
                {
                    TradeName = s.Medicines.TradeName,
                    ActiveIngredient = s.Medicines.ActiveIngredient,
                    BatchNumber = s.BatchNumber,
                    Quantity = s.Quantity,
                    ImportPrice = s.ImportPrice,
                    ManufactureDate = s.ManufactureDate,
                    ExpiryDate = s.ExpiryDate,
                    ImportDate = s.ImportDate,
                    ManufacturerName = s.Medicines.Manufacturers.Name,
                    SupplierName = s.Suppliers.SupplierName
                }).ToList();

            var totalItemsExpired = queryExpired.Count();
            var pagedExpired = queryExpired.OrderBy(s => s.BatchNumber)
                .Skip((expiredPage - 1) * sizePerPage)
                .Take(sizePerPage)
                .Select(s => new StorageReportItem
                {
                    ReceiptCode = s.ReceiptCode,
                    TradeName = s.Medicines.TradeName,
                    ActiveIngredient = s.Medicines.ActiveIngredient,
                    BatchNumber = s.BatchNumber,
                    Quantity = s.Quantity,
                    ManufactureDate = s.ManufactureDate,
                    ExpiryDate = s.ExpiryDate,
                    ImportDate = s.ImportDate
                }).ToList();

            var totalItemsNearExpiry = queryNearExpiry.Count();
            var pagedNearExpiry = queryNearExpiry.OrderBy(s => s.BatchNumber)
                .Skip((nearExpiryPage - 1) * sizePerPage)
                .Take(sizePerPage)
                .Select(s => new StorageReportItem
                {
                    ReceiptCode = s.ReceiptCode,
                    TradeName = s.Medicines.TradeName,
                    ActiveIngredient = s.Medicines.ActiveIngredient,
                    BatchNumber = s.BatchNumber,
                    Quantity = s.Quantity,
                    ManufactureDate = s.ManufactureDate,
                    ExpiryDate = s.ExpiryDate,
                    ImportDate = s.ImportDate
                }).ToList();

            // ViewModel
            var viewModel = new StorageReportViewModel
            {
                InventoryReport = pagedInventory,
                ExpiredReport = pagedExpired,
                NearExpiryReport = pagedNearExpiry
            };

            // Pagination Info
            ViewBag.CurrentPageInventory = inventoryPage;
            ViewBag.CurrentPageExpired = expiredPage;
            ViewBag.CurrentPageNearExpiry = nearExpiryPage;
            ViewBag.TotalPagesInventory = (int)Math.Ceiling((double)totalItemsInventory / sizePerPage);
            ViewBag.TotalPagesExpired = (int)Math.Ceiling((double)totalItemsExpired / sizePerPage);
            ViewBag.TotalPagesNearExpiry = (int)Math.Ceiling((double)totalItemsNearExpiry / sizePerPage);

            // ✅ TRẢ VỀ THEO viewName (nếu có)
            switch (viewName)
            {
                case "Inventory":
                    return PartialView("_ReportInventory", viewModel.InventoryReport);
                case "Expired":
                    return PartialView("_ReportExpired", viewModel.ExpiredReport);
                case "NearExpiry":
                    return PartialView("_ReportNearExpiry", viewModel.NearExpiryReport);
                default:
                    return View(viewModel); // Load đầy đủ ban đầu
            }
        }

        public ActionResult NearExpiryReport(int months = 3, string searchStr = "", int page = 1)
        {
            var now = DateTime.Now;
            var nearExpiryThreshold = now.AddMonths(months);

            // Query the data with optional search
            var queryNearExpiry = db.Storages
                .Where(s => s.ExpiryDate >= now && s.ExpiryDate <= nearExpiryThreshold)
                .Include(s => s.Medicines);

            // Apply Search Filter
            if (!string.IsNullOrEmpty(searchStr))
            {
                queryNearExpiry = queryNearExpiry.Where(s => s.Medicines.TradeName.Contains(searchStr) || s.Suppliers.SupplierName.Contains(searchStr));
            }

            // Pagination Controller
            int sizePerPage = 13;
            var totalItemsNearExpiry = queryNearExpiry.Count();
            var pagedNearExpiry = queryNearExpiry.OrderBy(s => s.BatchNumber)
                .Skip((page - 1) * sizePerPage)
                .Take(sizePerPage)
                .Select(s => new StorageReportItem
                {
                    ReceiptCode = s.ReceiptCode,
                    TradeName = s.Medicines.TradeName,
                    ActiveIngredient = s.Medicines.ActiveIngredient,
                    BatchNumber = s.BatchNumber,
                    Quantity = s.Quantity,
                    ManufactureDate = s.ManufactureDate,
                    ExpiryDate = s.ExpiryDate,
                    ImportDate = s.ImportDate
                }).ToList();

            // Return PartialView with paged data
            ViewBag.TotalPagesNearExpiry = (int)Math.Ceiling((double)totalItemsNearExpiry / sizePerPage);
            ViewBag.CurrentPageNearExpiry = page;
            return PartialView("_ReportNearExpiry", pagedNearExpiry);
        }

        // GET: Reports/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
