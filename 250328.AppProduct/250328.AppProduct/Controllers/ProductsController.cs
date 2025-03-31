using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _250328.AppProduct.Models;

namespace _250328.AppProduct.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            List<Products> productsList = new List<Products> ();

            Products pr = new Products();
            pr.ProductID = 1;
            pr.ProductName = "BookABC";
            pr.CategoryID = 1;
            pr.SuplierID = 1;
            pr.QuantityPerUnit = 10;
            pr.UnitPrice = 200000;
            pr.UnitsInStock = "Box";
            pr.UnitsOnOrder = "Book";
            pr.ReorderLevel = 2;
            pr.Discontinued = false;
            productsList.Add(pr);

            pr = new Products();
            pr.ProductID = 2;
            pr.ProductName = "BookDEF";
            pr.CategoryID = 1;
            pr.SuplierID = 1;
            pr.QuantityPerUnit = 10;
            pr.UnitPrice = 250000;
            pr.UnitsInStock = "Box";
            pr.UnitsOnOrder = "Book";
            pr.ReorderLevel = 2;
            pr.Discontinued = false;
            productsList.Add(pr);

            pr = new Products();
            pr.ProductID = 3;
            pr.ProductName = "BookGHI";
            pr.CategoryID = 1;
            pr.SuplierID = 1;
            pr.QuantityPerUnit = 10;
            pr.UnitPrice = 300000;
            pr.UnitsInStock = "Box";
            pr.UnitsOnOrder = "Book";
            pr.ReorderLevel = 2;
            pr.Discontinued = false;
            productsList.Add(pr);

            ViewBag.ProductsList = productsList;
            ViewBag.Product = pr;
            return View(productsList);
        }

        public ActionResult Update()
        {
            Products pr = new Products();
            pr.ProductID = 3;
            pr.ProductName = "BookGHI";
            pr.CategoryID = 1;
            pr.SuplierID = 1;
            pr.QuantityPerUnit = 10;
            pr.UnitPrice = 300000;
            pr.UnitsInStock = "Box";
            pr.UnitsOnOrder = "Book";
            pr.ReorderLevel = 2;
            pr.Discontinued = false;

            return View(pr);
        }
    }
}