using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework.Models
{
    public class Products
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Supplier is required")]
        public int? SupplierID { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int? CategoryID { get; set; }
        [Required(ErrorMessage = "Quantity Per Unit is required")]
        public string QuantityPerUnit { get; set; }
        [Required(ErrorMessage = "Unit Price is required")]
        public decimal UnitPrice { get; set; }
        [Required(ErrorMessage = "Units In Stock is required")]
        public short UnitsInStock { get; set; }
        [Required(ErrorMessage = "Units On Order is required")]
        public short UnitsOnOrder { get; set; }
        [Required(ErrorMessage = "Reorder Level is required")]
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public string Supplier { get; set; }
        public string Category { get; set; }
    }
}