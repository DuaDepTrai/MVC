using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FistAppMVC.Models;

namespace FistAppMVC.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            List <Products> products = new List<Products> ();

            Products pr = new Products();
            pr.ProductID = 1;
            pr.ProductName = "Bàn học sinh viên";
            pr.CategoryID = 1;
            pr.SupplierID = 1;
            pr.QuantityPerUnit = "Cái";
            pr.UnitPrice = 2000000;
            pr.UnitsInStock = 10;
            pr.UnitsOnOrder = 10;
            pr.ReorderLevel = 10;
            pr.Discontinued = true;

            products.Add (pr);

            pr = new Products();
            pr.ProductID = 2;
            pr.ProductName = "Ghế ngồi sinh viên";
            pr.CategoryID = 1;
            pr.SupplierID = 1;
            pr.QuantityPerUnit = "Cái";
            pr.UnitPrice = 2000000;
            pr.UnitsInStock = 10;
            pr.UnitsOnOrder = 10;
            pr.ReorderLevel = 10;
            pr.Discontinued = true;

            products.Add(pr);

            pr = new Products();
            pr.ProductID = 3;
            pr.ProductName = "Máy tính văn phòng";
            pr.CategoryID = 1;
            pr.SupplierID = 1;
            pr.QuantityPerUnit = "Cái";
            pr.UnitPrice = 2000000;
            pr.UnitsInStock = 10;
            pr.UnitsOnOrder = 10;
            pr.ReorderLevel = 10;
            pr.Discontinued = true;
            products.Add(pr);

            ViewBag.Products = products;
            ViewBag.Product = pr;
            return View(products);
        }


        public ActionResult Edit()
        {
            Products pr = new Products();
            pr.ProductID = 3;
            pr.ProductName = "Máy tính văn phòng";
            pr.CategoryID = 1;
            pr.SupplierID = 1;
            pr.QuantityPerUnit = "Cái";
            pr.UnitPrice = 2000000;
            pr.UnitsInStock = 10;
            pr.UnitsOnOrder = 10;
            pr.ReorderLevel = 10;
            pr.Discontinued = true;

            return View(pr);
        }
    }
}