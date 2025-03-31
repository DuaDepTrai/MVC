using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;

namespace Homework.Controllers
{
    public class CustomersController : Controller
    {
        static List<Customers> customers = new List<Customers>();
        // GET: Customers
        public ActionResult Index()
        {
            return View(customers);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Customers cus) 
        {
            customers.Add(cus);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            Customers cus = new Customers();

            foreach (Customers c in customers) 
            {
                if (c.CustomerID == id) 
                {
                    cus = c;                
                }
            }
            return View(cus);
        }

        [HttpPost]
        public ActionResult Edit(Customers cus)
        {
            int index = customers.FindIndex(n => n.CustomerID == cus.CustomerID);
            if (index != 01) 
            {
                customers[index] = cus;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            Customers cus = new Customers();

            foreach (Customers c in customers)
            {
                if (c.CustomerID == id)
                {
                    cus = c;
                }
            }
            return View(cus);
        }

        public ActionResult Delete(string id)
        {
            Customers cus = new Customers();

            foreach (Customers c in customers)
            {
                if (c.CustomerID == id)
                {
                    cus = c;
                }
            }
            return View(cus);
        }

        [HttpPost]
        public ActionResult Delete(Customers cus)
        {
            int index = customers.FindIndex(n => n.CustomerID == cus.CustomerID);
            Customers customer = customers[index];
            customers.Remove(customer);
            return RedirectToAction("Index");
        }
    }
}