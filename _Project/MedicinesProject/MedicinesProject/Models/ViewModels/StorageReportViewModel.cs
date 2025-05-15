using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicinesProject.Models.ViewModels
{
    public class StorageReportViewModel
    {
        public List<StorageReportItem> InventoryReport { get; set; }
        public List<StorageReportItem> ExpiredReport { get; set; }
        public List<StorageReportItem> NearExpiryReport { get; set; }
    }
}