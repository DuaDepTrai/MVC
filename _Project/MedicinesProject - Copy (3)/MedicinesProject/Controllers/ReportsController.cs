using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MedicinesProject.Models;
using MedicinesProject.Models.ViewModels;

namespace MedicinesProject.Controllers
{
    public class ReportsController : Controller
    {
        private MedicinesManagementEntities db = new MedicinesManagementEntities();

        // GET: Reports
        public ActionResult Index(string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? page)
        {
            const int pageSize = 15;
            int pageNumber = page ?? 1;

            ViewBag.CurrentSearch = searchStr;
            ViewBag.ActiveIngredientFilter = activeIngredientFilter;
            ViewBag.ManufacturerFilter = manufacturerFilter;
            ViewBag.SupplierFilter = supplierFilter;

            // SelectList
            ViewBag.ActiveIngredients = new SelectList(
                db.Medicines.Select(m => m.ActiveIngredient).Distinct().OrderBy(x => x).ToList(),
                "Filter Active Ingredients"
            );
            ViewBag.Manufacturers = new SelectList(
                db.Manufacturers.Select(m => m.Name).Distinct().OrderBy(x => x).ToList(),
                "Filter Manufacturers"
            );
            ViewBag.Suppliers = new SelectList(
                db.Suppliers.Select(s => s.SupplierName).Distinct().OrderBy(x => x).ToList(),
                "Filter Suppliers"
            );
            var medicines = db.Storages
                              .Where(s => s.ExpiryDate >= DateTime.Now)
                              .Join(db.Medicines, s => s.MedicineID, m => m.MedicineID, (s, m) => new InventoryReportViewModel
                              {
                                  ReceiptCode = s.ReceiptCode,
                                  TradeName = m.TradeName,
                                  ActiveIngredient = m.ActiveIngredient,
                                  BatchNumber = s.BatchNumber,
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
            if (!string.IsNullOrEmpty(manufacturerFilter))
            {
                medicines = medicines.Where(m => m.ManufacturerName == manufacturerFilter);
            }
            if (!string.IsNullOrEmpty(supplierFilter))
            {
                medicines = medicines.Where(m => m.SupplierName == supplierFilter);
            }

            var totalRecords = medicines.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            medicines = medicines
                        .OrderBy(m => m.ActiveIngredient)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(medicines.ToList());
        }

        public PartialViewResult Inventory(string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? page)
        {
            const int pageSize = 15;
            int pageNumber = page ?? 1;

            ViewBag.CurrentSearch = searchStr;
            ViewBag.ActiveIngredientFilter = activeIngredientFilter;
            ViewBag.ManufacturerFilter = manufacturerFilter;
            ViewBag.SupplierFilter = supplierFilter;

            //SelectList: Dùng danh sách chuỗi trực tiếp
            ViewBag.ActiveIngredients = new SelectList(
                db.Medicines.Select(m => m.ActiveIngredient).Distinct().OrderBy(x => x).ToList(),
                "Filter Active Ingredients"
            );
            ViewBag.Manufacturers = new SelectList(
                db.Manufacturers.Select(m => m.Name).Distinct().OrderBy(x => x).ToList(),
                "Filter Manufacturers"
            );
            ViewBag.Suppliers = new SelectList(
                db.Suppliers.Select(s => s.SupplierName).Distinct().OrderBy(x => x).ToList(),
                "Filter Suppliers"
            );
            var medicines = db.Storages
                              .Where(s => s.ExpiryDate >= DateTime.Now)
                              .Join(db.Medicines, s => s.MedicineID, m => m.MedicineID, (s, m) => new InventoryReportViewModel
                              {
                                  ReceiptCode = s.ReceiptCode,
                                  TradeName = m.TradeName,
                                  ActiveIngredient = m.ActiveIngredient,
                                  BatchNumber = s.BatchNumber,
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
            if (!string.IsNullOrEmpty(manufacturerFilter))
            {
                medicines = medicines.Where(m => m.ManufacturerName == manufacturerFilter);
            }
            if (!string.IsNullOrEmpty(supplierFilter))
            {
                medicines = medicines.Where(m => m.SupplierName == supplierFilter);
            }

            var totalRecords = medicines.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            medicines = medicines
                        .OrderBy(m => m.ActiveIngredient)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return PartialView("_Inventory", medicines.ToList());
        }

        public PartialViewResult Expired(string searchStr, string activeIngredientFilter, int? page)
        {
            const int pageSize = 15;
            int pageNumber = page ?? 1;

            try
            {
                ViewBag.CurrentSearch = searchStr;
                ViewBag.ActiveIngredientFilter = activeIngredientFilter;
                ViewBag.Manufacturers = new SelectList(Enumerable.Empty<string>());
                ViewBag.Suppliers = new SelectList(Enumerable.Empty<string>());

                //SelectList: Dùng danh sách chuỗi trực tiếp
                ViewBag.ActiveIngredients = new SelectList(
                    db.Medicines.Select(m => m.ActiveIngredient).Distinct().OrderBy(x => x).ToList(),
                    "Filter Active Ingredients"
                );
                
                var expiredMedicines = db.Storages
                                      .Where(s => s.ExpiryDate < DateTime.Now)
                                      .Join(db.Medicines, s => s.MedicineID, m => m.MedicineID, (s, m) => new InventoryReportViewModel
                                      {
                                          ReceiptCode = s.ReceiptCode,
                                          TradeName = m.TradeName,
                                          ActiveIngredient = m.ActiveIngredient,
                                          BatchNumber = s.BatchNumber,
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
                    expiredMedicines = expiredMedicines.Where(m => m.ReceiptCode.ToLower().Contains(searchStr) ||
                                                                 m.TradeName.ToLower().Contains(searchStr) ||
                                                                 m.ActiveIngredient.ToLower().Contains(searchStr) ||
                                                                 m.BatchNumber.ToLower().Contains(searchStr));
                }

                if (!string.IsNullOrEmpty(activeIngredientFilter))
                {
                    expiredMedicines = expiredMedicines.Where(m => m.ActiveIngredient == activeIngredientFilter);
                }

                var totalRecords = expiredMedicines.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                expiredMedicines = expiredMedicines
                                   .OrderByDescending(e => e.ExpiryDate)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize);

                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;

                return PartialView("_Expired", expiredMedicines.ToList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in Expired: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public ActionResult NearExpiry(string searchStr, string activeIngredientFilter, int? months, int? page)
        {
            const int pageSize = 14;
            int pageNumber = page ?? 1;

            int expiryMonths = months.HasValue && months.Value > 0 ? months.Value : 3;

            var now = DateTime.Now;
            var monthsLater = now.AddMonths(expiryMonths);
            var medicines = db.Storages
                             .Where(s => s.ExpiryDate >= now && s.ExpiryDate <= monthsLater)
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
                medicines = medicines.Where(m => (m.ReceiptCode != null && m.ReceiptCode.ToLower().Contains(searchStr)) ||
                                               (m.TradeName != null && m.TradeName.ToLower().Contains(searchStr)) ||
                                               (m.ActiveIngredient != null && m.ActiveIngredient.ToLower().Contains(searchStr)) ||
                                               (m.BatchNumber != null && m.BatchNumber.ToLower().Contains(searchStr)));
            }

            if (!string.IsNullOrEmpty(activeIngredientFilter))
            {
                medicines = medicines.Where(m => m.ActiveIngredient == activeIngredientFilter);
            }

            var totalRecords = medicines.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            medicines = medicines
                        .OrderBy(e => e.ExpiryDate)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);

            ViewBag.CurrentSearch = searchStr;
            ViewBag.ActiveIngredientFilter = activeIngredientFilter;
            ViewBag.Months = expiryMonths;
            ViewBag.ActiveIngredients = new SelectList(
                db.Medicines.Select(m => m.ActiveIngredient).Distinct().OrderBy(x => x).ToList(),
                "Filter Active Ingredients"
            );
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return PartialView("_NearExpiry", medicines.ToList());
        }

        public ActionResult ExportToExcel(string reportType, string searchStr, string activeIngredientFilter, string manufacturerFilter, string supplierFilter, int? months)
        {
            try
            {
                // Kiểm tra reportType
                if (string.IsNullOrEmpty(reportType))
                {
                    System.Diagnostics.Debug.WriteLine("ExportToExcel: reportType is null or empty");
                    return new HttpStatusCodeResult(400, "Report type is required");
                }

                IQueryable<InventoryReportViewModel> medicines;

                // Xác định dữ liệu dựa trên reportType
                switch (reportType.ToLower())
                {
                    case "inventory":
                        medicines = db.Storages
                                     .Where(s => s.ExpiryDate >= DateTime.Now)
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
                        break;

                    case "expired":
                        medicines = db.Storages
                                     .Where(s => s.ExpiryDate < DateTime.Now)
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
                        break;

                    case "nearexpiry":
                        var now = DateTime.Now;
                        // Mặc định 3 tháng nếu không có giá trị
                        int expiryMonths = months.HasValue && months.Value > 0 ? months.Value : 3;
                        var monthsLater = now.AddMonths(expiryMonths);
                        medicines = db.Storages
                                     .Where(s => s.ExpiryDate >= now && s.ExpiryDate <= monthsLater)
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
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine($"ExportToExcel: Invalid reportType: {reportType}");
                        return new HttpStatusCodeResult(400, "Invalid report type");
                }

                // Áp dụng tìm kiếm
                if (!string.IsNullOrEmpty(searchStr))
                {
                    searchStr = searchStr.ToLower();
                    medicines = medicines.Where(m => (m.ReceiptCode != null && m.ReceiptCode.ToLower().Contains(searchStr)) ||
                                                   (m.TradeName != null && m.TradeName.ToLower().Contains(searchStr)) ||
                                                   (m.ActiveIngredient != null && m.ActiveIngredient.ToLower().Contains(searchStr)) ||
                                                   (m.BatchNumber != null && m.BatchNumber.ToLower().Contains(searchStr)));
                }

                // Áp dụng lọc
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

                var data = medicines.ToList();

                // Kiểm tra dữ liệu
                System.Diagnostics.Debug.WriteLine($"ExportToExcel: Retrieved {data.Count} records for {reportType}");

                // Tạo file Excel với ClosedXML
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Report");
                    // Tiêu đề cột
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

                    // Đổ dữ liệu
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

                    // Tự động điều chỉnh cột
                    worksheet.Columns().AdjustToContents();

                    // Tạo file Excel
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;
                        var fileName = $"{reportType}_{DateTime.Now.ToString("yyyyMMdd")}.xlsx";

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
