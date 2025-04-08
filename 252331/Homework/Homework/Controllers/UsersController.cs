using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Homework.Controllers
{
    public class UsersController : BaseController
    {
        string strcnn = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        List<Users> users = new List<Users>();
        // GET: Users
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            string Sql = "SELECT USERS.UserID, USERS.UserName, USERS.Password, USERS.Discription, USERS.EmployeeID, Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM USERS LEFT OUTER JOIN Employees ON USERS.EmployeeID = Employees.EmployeeID";
            da = new SqlDataAdapter(Sql, conn);
            da.Fill(ds, "USERS");

            users.Clear();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Users user = new Users();
                user.UserID = int.Parse(item["UserID"].ToString());
                user.UserName = item["UserName"].ToString();
                user.FullName = item["FullName"].ToString();
                user.Description = item["Discription"].ToString();
                users.Add(user);
            }

            return View(users);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            Users user = new Users();
            string Sql = "SELECT USERS.UserID, USERS.UserName, USERS.Password, USERS.Discription, USERS.EmployeeID, Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM USERS LEFT OUTER JOIN Employees ON USERS.EmployeeID = Employees.EmployeeID " +
                "WHERE UserID=" + id;

            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                user.UserID = (int)dt.Rows[0]["UserID"];
                user.UserName = dt.Rows[0]["UserName"].ToString();
                user.FullName = dt.Rows[0]["FullName"].ToString();
                user.Description = dt.Rows[0]["Discription"].ToString();
            }
            users.Clear();

            return View(user);
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
        public ActionResult Login(Users user)
        {
            if (user != null) 
            {
                SqlConnection conn = new SqlConnection(strcnn);
                string Sql = "SELECT * FROM USERS " +
                    "WHERE UserName=N'"+ user.UserName +"' " +
                    "AND Password=N'"+ user.Password +"'";
                SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    if (user.Remember)
                    {
                        Response.Cookies["UserName"].Value = user.UserName;
                        Response.Cookies["UserName"].Expires = DateTime.MaxValue;
                    }
                    Session["UserID"] = dt.Rows[0]["UserID"];
                    Session["UserName"] = dt.Rows[0]["UserName"];
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Content("Invalid Username or Password");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Users");
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT EmployeeID, LastName + ' ' + FirstName AS FullName FROM Employees";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Employees> emps = new List<Employees>();
            foreach (DataRow item in dt.Rows)
            {
                Employees emp = new Employees();
                emp.EmployeeID = int.Parse(item["EmployeeID"].ToString());
                emp.FullName = item["FullName"].ToString();
                emps.Add(emp);
            }
            ViewBag.Employees = emps;

            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(Users obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj != null)
                {
                    string Sql = "INSERT INTO USERS (UserName, Password, Discription, EmployeeID) " +
                        "Values(N'" + obj.UserName + "', " +
                        "N'" + obj.Password + "', " +
                        "N'" + obj.Description + "', " +
                        "" + obj.EmployeeID + ")";
                    SqlConnection conn = new SqlConnection(strcnn);

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = Sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                users.Clear();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT EmployeeID, LastName + ' ' + FirstName AS FullName FROM Employees";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Employees> emps = new List<Employees>();
            foreach (DataRow item in dt.Rows)
            {
                Employees emp = new Employees();
                emp.EmployeeID = int.Parse(item["EmployeeID"].ToString());
                emp.FullName = item["FullName"].ToString();
                emps.Add(emp);
            }
            ViewBag.Employees = emps;

            Users user = new Users();
            Sql = "SELECT USERS.UserName, USERS.Password, USERS.Discription, USERS.EmployeeID, Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM USERS LEFT OUTER JOIN Employees ON USERS.EmployeeID = Employees.EmployeeID WHERE UserID=" + id;

            //string Sql = "SELECT * FROM USERS WHERE UserID=" + id;
            //SqlConnection conn = new SqlConnection(strcnn);
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                user.UserName = dt.Rows[0]["UserName"].ToString();
                user.FullName = dt.Rows[0]["FullName"].ToString();
                user.Description = dt.Rows[0]["Discription"].ToString();
                user.EmployeeID = int.Parse(dt.Rows[0]["EmployeeID"].ToString());
            }
            users.Clear();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Users obj)
        {
            try
            {
                // TODO: Add update logic here
                if (obj != null)
                {
                    string Sql = "UPDATE USERS " +
                        "SET UserName=N'" + obj.UserName + "', " +
                        "Password=N'" + obj.Password + "', " +
                        "Discription=N'" + obj.Description + "', " +
                        "EmployeeID=" + obj.EmployeeID + 
                        " WHERE UserID=" + id;
                    SqlConnection conn = new SqlConnection(strcnn);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = Sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                users.Clear();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;

                SqlConnection conn = new SqlConnection(strcnn);
                string Sql = "SELECT EmployeeID, LastName + ' ' + FirstName AS FullName FROM Employees";
                SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<Employees> emps = new List<Employees>();
                foreach (DataRow item in dt.Rows)
                {
                    Employees emp = new Employees();
                    emp.EmployeeID = int.Parse(item["EmployeeID"].ToString());
                    emp.FullName = item["FullName"].ToString();
                    emps.Add(emp);
                }
                ViewBag.Employees = emps;

                return View(obj);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            Users user = new Users();
            string Sql = "SELECT USERS.UserID, USERS.UserName, USERS.Password, USERS.Discription, USERS.EmployeeID, Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM USERS LEFT OUTER JOIN Employees ON USERS.EmployeeID = Employees.EmployeeID " +
                "WHERE UserID=" + id;

            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                user.UserID = (int)dt.Rows[0]["UserID"];
                user.UserName = dt.Rows[0]["UserName"].ToString();
                user.FullName = dt.Rows[0]["FullName"].ToString();
                user.Description = dt.Rows[0]["Discription"].ToString();
            }
            users.Clear();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Users obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null)
                {
                    string Sql = "DELETE FROM USERS WHERE UserID=" + id;
                    SqlConnection conn = new SqlConnection(strcnn);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = Sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                users.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
