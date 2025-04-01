using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _250324.App6.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Now=DateTime.Now;
            return View("~/Views/Home/nextView.cshtml");
        }

        public ActionResult Hello(string name)
        {
            ViewData["name"] = name;
            ViewData["now"] = DateTime.Now;
            return View();
        }

        public ActionResult Ex1(string name)
        {
            return Content($"Student's name: {name}");
        }

        public ActionResult Ex2(int no) 
        {
            if (no % 2 == 0) { return Content($"{no} is even"); }
            else { return Content($"{no} is odd"); }
        }

        public ActionResult Ex3(int a, int b, int c) 
        {
            if (a + b < c || a + c < b || b + c < a)
            {
                return Content("Not a triangle");
            }
            else
            {
                int p = (a + b + c)/2;
                double s = Math.Sqrt(p*(p-a)*(p-b)*(p-c));
                if (a == b && a == c)
                {
                    return Content($"Equilateral triangle - tam giac deu\tP = {p*2}\tS = {s}"); //deu
                }
                else
                {
                    if (a * a + b * b == c * c || a * a + c * c == b * b || b * b + c * c == a * a)
                    {
                        if (a == b || a == c || b == c)
                        {
                            return Content($"Isosceles right triangle - tam giac vuong can\nP = {p*2}\nS = {s}"); //vuong can
                        }
                        else
                        {
                            return Content($"Right triangle - tam giac vuong\nP = {p*2}\nS = {s}"); //vuong
                        }
                    }
                    else
                    {
                        if (a == b || a == c || b == c)
                        {
                            return Content($"Isosceles triangle - tam giac can\nP = {p*2}\nS = {s}"); //can
                        }
                        else
                        {
                            return Content($"Scalene triangle - tam giac thuong\nP = {p*2}\nS = {s}"); //thuong
                        }
                    }
                }
            }
        }

        public ActionResult Ex4(string s)
        {
            if (s.ToLower().Equals("a") || s.ToLower().Equals("e") || s.ToLower().Equals("i") || s.ToLower().Equals("o") || s.ToLower().Equals("or") || s.ToLower().Equals("u"))
            {
                return Content("This is a Vowel");
            }
            else 
            {
                return Content("This is not a Vowel");
            }
        }

        public ActionResult Ex5(int n)
        {
            int s1=0;
            double s2=0;
            for (int i=1; i<=n; i++)
            {
                s1=s1+i;
                s2=s2+1/(double)i;
            }
            return Content($"S1 = {s1}\nS2 = {s2}");
        }
    }
}