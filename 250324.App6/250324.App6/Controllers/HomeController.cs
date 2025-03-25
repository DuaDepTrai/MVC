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
            return Content("Dua Dep Trai");
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
                    return Content($"Equilateral triangle - tam giac deu\nP = {p*2}\nS = {s}"); //deu
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
            if (s.Equals("a") || s.Equals("a") || s.Equals("a") || s.Equals("a") || s.Equals("a") || s.Equals("a"))
        }
    }
}