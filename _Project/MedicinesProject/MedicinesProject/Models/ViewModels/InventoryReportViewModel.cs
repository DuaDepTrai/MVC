using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicinesProject.Models.ViewModels
{
    public class InventoryReportViewModel
    {
        public string ReceiptCode { get; set; }
        public string TradeName { get; set; }
        public string ActiveIngredient { get; set; }
        public string BatchNumber { get; set; }
        public int Quantity { get; set; }
        public decimal ImportPrice { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ImportDate { get; set; }
        public string ManufacturerName { get; set; }
        public string SupplierName { get; set; }
    }
}