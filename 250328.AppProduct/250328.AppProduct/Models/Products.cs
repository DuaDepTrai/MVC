using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _250328.AppProduct.Models
{
    public class Products
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public int SuplierID { get; set; }
        public int QuantityPerUnit { get; set; }
        public int UnitPrice { get; set; }
        public string UnitsInStock { get; set; }
        public string UnitsOnOrder { get; set; }
        public int ReorderLevel {  get; set; }
        public bool Discontinued { get; set; }
    }
}