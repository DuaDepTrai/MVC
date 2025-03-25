using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _250324.App6.Controllers
{
    public class Bai2Controller : Controller
    {
        // GET: Bai2
        public ActionResult GiaiPTB2(int a, int b, int c)
        {
            if (a == 0)
            {
                if (b == 0)
                {
                    if (c == 0)
                    {
                        return Content("PT vo so nghiem");
                    }
                    else
                    {
                        return Content("PT vo nghiem");
                    }
                }
                else
                {
                    return Content($"PT co nghiem x = {-c / b}");
                }
            }
            else
            {
                int d = b * b - 4 * a * c;
                if (d < 0)
                {
                    return Content("PT vo nghiem");
                }
                else
                {
                    if (d == 0) 
                    {
                        return Content($"PT co nghiem x = {-b / (2 * a)}");
                    }
                    else
                    {
                        return Content($"PT co 2 nghiem:\nx1 = {(-b + Math.Sqrt(d))/(2*a)}\nx2 = {(-b - Math.Sqrt(d)) / (2 * a)}");
                    }
                }
            }
        }

        public ActionResult Duongtron(int r)
        {
            return Content($"Chu vi duong tron: {2 * Math.PI * r}\nDien tich hinh tron: {Math.PI * r * r}");
        }

        public ActionResult TamGiac(int a,  int b, int c)
        {
            if ((a + b > c) && (b + c > a) && (c + a > b))
            {
                double p = (a + b + c) / 2;
                return Content($"Chu vi tam giac: {p * 2}\nDien tich tam giac: {Math.Sqrt(p * (p - a) * (p - b) * (p - c))}");
            }
            else
            {
                return Content("Khong phai tam giac");
            }
        }
    }
}