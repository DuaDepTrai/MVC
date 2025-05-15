using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using MedicinesProject.Models;
using MedicinesProject.Models.ViewModels;

namespace MedicinesProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly MedicinesManagementEntities db = new MedicinesManagementEntities();

        private const int InventoryPageSize = 15;
        private const int NearExpiryPageSize = 13;

        // GET: Reports
        public ActionResult Index(string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? page)
        {
            return GetReport("Inventory", searchStr, activeIngredientFilter, manufacturerFilter, supplierFilter, null, page);
        }

        public ActionResult Inventory(string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? page)
        {
            return GetReport("Inventory", searchStr, activeIngredientFilter, manufacturerFilter, supplierFilter, null, page);
        }

        public ActionResult Expired(string searchStr, string activeIngredientFilter, int? page)
        {
            return GetReport("Expired", searchStr, activeIngredientFilter, null, null, null, page);
        }

        public ActionResult NearExpiry(string searchStr, string activeIngredientFilter, int? months, int? page)
        {
            return GetReport("NearExpiry", searchStr, activeIngredientFilter, null, null, months, page);
        }

        private ActionResult GetReport(string reportType, string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? months, int? page)
        {
            try
            {
                int pageSize = reportType == "NearExpiry" ? NearExpiryPageSize : InventoryPageSize;
                int pageNumber = page ?? 1;

                // Chuẩn bị ViewBag
                SetupViewBag(searchStr, activeIngredientFilter, manufacturerFilter, supplierFilter, months, pageNumber, reportType);

                // Lấy dữ liệu
                var medicines = GetFilteredMedicines(reportType, searchStr, activeIngredientFilter, manufacturerFilter, supplierFilter, months);

                // Phân trang
                var totalRecords = medicines.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                medicines = ApplyPaging(medicines, reportType, pageNumber, pageSize);
                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;

                var model = medicines.ToList();

                // Xử lý AJAX hoặc truy cập trực tiếp
                if (Request.IsAjaxRequest())
                {
                    string partialViewName = "_" + reportType;
                    return PartialView(partialViewName, model);
                }

                ViewBag.PartialViewName = "_" + reportType;
                return View("Index", model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetReport ({reportType}): {ex.Message}\n{ex.StackTrace}");
                return new HttpStatusCodeResult(500, "Error processing report");
            }
        }

        private void SetupViewBag(string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? months, int pageNumber, string reportType)
        {
            ViewBag.CurrentSearch = searchStr;
            ViewBag.ActiveIngredientFilter = activeIngredientFilter;
            ViewBag.ManufacturerFilter = manufacturerFilter;
            ViewBag.SupplierFilter = supplierFilter;
            ViewBag.Months = months ?? 3;
            ViewBag.CurrentPage = pageNumber;

            ViewBag.ActiveIngredients = new SelectList(
                db.Medicines.Select(m => m.ActiveIngredient).Distinct().OrderBy(x => x).ToList(),
                activeIngredientFilter
            );
            if (reportType == "Expired" || reportType == "NearExpiry")
            {
                ViewBag.Manufacturers = new SelectList(Enumerable.Empty<string>());
                ViewBag.Suppliers = new SelectList(Enumerable.Empty<string>());
            }
            else
            {
                ViewBag.Manufacturers = new SelectList(
                    db.Manufacturers.Select(m => m.Name).Distinct().OrderBy(x => x).ToList(),
                    manufacturerFilter
                );
                ViewBag.Suppliers = new SelectList(
                    db.Suppliers.Select(s => s.SupplierName).Distinct().OrderBy(x => x).ToList(),
                    supplierFilter
                );
            }
        }

        private IQueryable<InventoryReportViewModel> GetFilteredMedicines(string reportType, string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? months)
        {
            var now = DateTime.Now;
            IQueryable<Storages> storages;

            switch (reportType.ToLower())
            {
                case "inventory":
                    storages = db.Storages.Where(s => s.ExpiryDate >= now);
                    break;
                case "expired":
                    storages = db.Storages.Where(s => s.ExpiryDate < now);
                    break;
                case "nearexpiry":
                    var monthsLater = now.AddMonths(months ?? 3);
                    storages = db.Storages.Where(s => s.ExpiryDate >= now && s.ExpiryDate <= monthsLater);
                    break;
                default:
                    throw new ArgumentException("Invalid report type");
            }

            var medicines = storages
                .Join(db.Medicines, s => s.MedicineID, m => m.MedicineID, (s, m) => new InventoryReportViewModel
                {
                    ReceiptCode = s.ReceiptCode ?? "N/A",
                    TradeName = m.TradeName ?? "N/A",
                    ActiveIngredient = m.ActiveIngredient ?? "N/A",
                    BatchNumber = s.BatchNumber ?? "N/A",
                    Quantity = s.Quantity,
                    ImportPrice = s.ImportPrice,
                    ManufactureDate = s.ManufactureDate,
                    ExpiryDate = s.ExpiryDate,
                    ImportDate = s.ImportDate,
                    ManufacturerName = m.Manufacturers != null ? m.Manufacturers.Name : "N/A",
                    SupplierName = s.Suppliers != null ? s.Suppliers.SupplierName : "N/A"
                });

            if (!string.IsNullOrEmpty(searchStr))
            {
                searchStr = searchStr.ToLower();
                medicines = medicines.Where(m => m.ReceiptCode.ToLower().Contains(searchStr) ||
                                               m.TradeName.ToLower().Contains(searchStr) ||
                                               m.ActiveIngredient.ToLower().Contains(searchStr) ||
                                               m.BatchNumber.ToLower().Contains(searchStr));
            }

            if (!string.IsNullOrEmpty(activeIngredientFilter))
            {
                medicines = medicines.Where(m => m.ActiveIngredient == activeIngredientFilter);
            }

            if (!string.IsNullOrEmpty(manufacturerFilter) && reportType.ToLower() == "inventory")
            {
                medicines = medicines.Where(m => m.ManufacturerName == manufacturerFilter);
            }

            if (!string.IsNullOrEmpty(supplierFilter) && reportType.ToLower() == "inventory")
            {
                medicines = medicines.Where(m => m.SupplierName == supplierFilter);
            }

            return medicines;
        }

        private IQueryable<InventoryReportViewModel> ApplyPaging(IQueryable<InventoryReportViewModel> medicines, string reportType, int pageNumber, int pageSize)
        {
            switch (reportType.ToLower())
            {
                case "inventory":
                    return medicines.OrderBy(m => m.ActiveIngredient).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                case "expired":
                    return medicines.OrderByDescending(m => m.ExpiryDate).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                case "nearexpiry":
                    return medicines.OrderBy(m => m.ExpiryDate).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                default:
                    return medicines;
            }
        }

        public ActionResult ExportToExcel(string reportType, string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? months)
        {
            try
            {
                if (string.IsNullOrEmpty(reportType))
                {
                    System.Diagnostics.Debug.WriteLine("ExportToExcel: reportType is null or empty");
                    return new HttpStatusCodeResult(400, "Report type is required");
                }

                var medicines = GetFilteredMedicines(reportType, searchStr, activeIngredientFilter, manufacturerFilter, supplierFilter, months);
                var data = medicines.ToList();

                System.Diagnostics.Debug.WriteLine($"ExportToExcel: Retrieved {data.Count} records for {reportType}");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Report");
                    worksheet.Cell(1, 1).Value = "Receipt Code";
                    worksheet.Cell(1, 2).Value = "Trade Name";
                    worksheet.Cell(1, 3).Value = "Active Ingredient";
                    worksheet.Cell(1, 4).Value = "Batch Number";
                    worksheet.Cell(1, 5).Value = "Quantity";
                    worksheet.Cell(1, 6).Value = "Import Price";
                    worksheet.Cell(1, 7).Value = "Manufacture Date";
                    worksheet.Cell(1, 8).Value = "Expiry Date";
                    worksheet.Cell(1, 9).Value = "Import Date";
                    worksheet.Cell(1, 10).Value = "Manufacturer";
                    worksheet.Cell(1, 11).Value = "Supplier";

                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = data[i].ReceiptCode;
                        worksheet.Cell(i + 2, 2).Value = data[i].TradeName;
                        worksheet.Cell(i + 2, 3).Value = data[i].ActiveIngredient;
                        worksheet.Cell(i + 2, 4).Value = data[i].BatchNumber;
                        worksheet.Cell(i + 2, 5).Value = data[i].Quantity;
                        worksheet.Cell(i + 2, 6).Value = data[i].ImportPrice;
                        worksheet.Cell(i + 2, 7).Value = data[i].ManufactureDate.ToString("dd/MM/yyyy");
                        worksheet.Cell(i + 2, 8).Value = data[i].ExpiryDate.ToString("dd/MM/yyyy");
                        worksheet.Cell(i + 2, 9).Value = data[i].ImportDate.ToString("dd/MM/yyyy");
                        worksheet.Cell(i + 2, 10).Value = data[i].ManufacturerName;
                        worksheet.Cell(i + 2, 11).Value = data[i].SupplierName;
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;
                        var fileName = $"{reportType}_{DateTime.Now:yyyyMMdd}.xlsx";
                        System.Diagnostics.Debug.WriteLine($"ExportToExcel: Generated Excel file {fileName}");
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ExportToExcel: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new HttpStatusCodeResult(500, "Error generating Excel file");
            }
        }
    }
}