using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FistAppMVC.Models;
namespace FistAppMVC.Controllers
{
    public class UsersController : Controller
    {
        string strcnn = ConfigurationManager.ConnectionStrings["Chuoiketnoi"].ConnectionString;

        // GET: Users
        public ActionResult Index()
        {
            List<Users> listusser = new List<Users>();

            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT USERS.UserID, USERS.UserName, USERS.Password, USERS.Discription, USERS.EmployeeID, Employees.LastName + ' ' + Employees.FirstName AS FullName FROM  USERS LEFT OUTER JOIN Employees ON USERS.EmployeeID = Employees.EmployeeID ";

            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Users users = new Users();
                users.UserID = int.Parse(item["UserID"].ToString());
                users.UserName = item["UserName"].ToString();
                users.Description = item["Discription"].ToString();
                users.FullName = item["FullName"].ToString();
                listusser.Add(users);

            }


            return View(listusser);
        }

        // GET: Users
        public ActionResult Login()
        {
            Users users = new Users();
            if (Request.Cookies["UserName"] != null)
            {
                users.UserName = Request.Cookies["UserName"].Value;
            }

            return View(users);
        }
        [HttpPost]
        public ActionResult Login(Users us)
        {
            if (us != null)
            {
                SqlConnection conn = new SqlConnection(strcnn);
                string Sql = "SELECT * FROM USERS WHERE UserName=N'" + us.UserName + "' AND Password=N'" + us.Password + "'";
                SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (us.Remember)
                    {
                        Response.Cookies["UserName"].Value = us.UserName;
                        Response.Cookies["UserName"].Expires = DateTime.MaxValue;
                    }

                    Session["UserID"] = dt.Rows[0]["UserID"];
                    Session["UserName"] = dt.Rows[0]["UserName"];
                    Session["EmployeeID"] = dt.Rows[0]["EmployeeID"];

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Content("Bạn sai UserName hoặc Password");
                }
            }

            return View();
        }
        public ActionResult Create()
        {

            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT EmployeeID, LastName + ' ' + FirstName AS FullName FROM Employees";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Employees> listemp = new List<Employees>();

            foreach (DataRow dr in dt.Rows)
            {
                Employees emp = new Employees();
                emp.EmployeeID = int.Parse(dr["EmployeeID"].ToString());
                emp.FullName = dr["FullName"].ToString();
                listemp.Add(emp);
            }
            ViewBag.Employees = listemp;


            return View();
        }

        [HttpPost]
        public ActionResult Create(Users obj)
        {
            if (!ModelState.IsValid) {
                SqlConnection conn = new SqlConnection(strcnn);
                string Sql = "SELECT EmployeeID, LastName + ' ' + FirstName AS FullName FROM Employees";
                SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                List<Employees> listemp = new List<Employees>();

                foreach (DataRow dr in dt.Rows)
                {
                    Employees emp = new Employees();
                    emp.EmployeeID = int.Parse(dr["EmployeeID"].ToString());
                    emp.FullName = dr["FullName"].ToString();
                    listemp.Add(emp);
                }
                ViewBag.Employees = listemp;
                return View(obj);
            }

            


            return RedirectToAction("Index");
        }
    }
}