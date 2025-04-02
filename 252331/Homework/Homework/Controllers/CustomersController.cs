using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;

namespace Homework.Controllers
{
    public class CustomersController : Controller
    {
        string strcnn = "Server=DESKTOP-A5P4OVC\\SQLEXPRESS;Database=Northwind;Integrated Security=True;";
        static List<Customers> customers = new List<Customers>();
        // GET: Customers
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            string Sql = "SELECT * FROM Customers";
            da = new SqlDataAdapter(Sql, conn);
            da.Fill(ds, "Customers");

            foreach (DataRow item in ds.Tables[0].Rows) 
            {
                Customers cus = new Customers();
                cus.CustomerID = item["CustomerID"].ToString();
                cus.CompanyName = item["CompanyName"].ToString();
                cus.ContactName = item["ContactName"].ToString();
                cus.ContactTitle = item["ContactTitle"].ToString();
                cus.Address = item["Address"].ToString();
                cus.City = item["City"].ToString();
                cus.Region = item["Region"].ToString();
                cus.PostalCode = item["PostalCode"].ToString();
                cus.Country = item["Country"].ToString();
                cus.Phone = item["Phone"].ToString();
                cus.Fax = item["Fax"].ToString();
                customers.Add(cus);
            }
            return View(customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            Customers cus = new Customers();
            string Sql = "SELECT * FROM Customers WHERE CustomerID=N'" + id + "'";
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cus.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                cus.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                cus.ContactName = dt.Rows[0]["ContactName"].ToString();
                cus.ContactTitle = dt.Rows[0]["ContactTitle"].ToString();
                cus.Address = dt.Rows[0]["Address"].ToString();
                cus.City = dt.Rows[0]["City"].ToString();
                cus.Region = dt.Rows[0]["Region"].ToString();
                cus.PostalCode = dt.Rows[0]["PostalCode"].ToString();
                cus.Country = dt.Rows[0]["Country"].ToString();
                cus.Phone = dt.Rows[0]["Phone"].ToString();
                cus.Fax = dt.Rows[0]["Fax"].ToString();
            }
            customers.Clear();
            return View(cus);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customers obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj != null)
                {
                    string Sql = "INSERT INTO Customers (CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) " +
                        "Values(N'" + obj.CustomerID + "'," +
                        " N'" + obj.CompanyName + "'," +
                        " N'" + obj.ContactName + "'," +
                        " N'" + obj.ContactTitle + "'," +
                        " N'" + obj.Address + "'," +
                        " N'" + obj.City + "'," +
                        " N'" + obj.Region + "'," +
                        " N'" + obj.PostalCode + "'," +
                        " N'" + obj.Country + "'," +
                        " N'" + obj.Phone + "'," +
                        " N'" + obj.Fax + "')";
                    SqlConnection conn = new SqlConnection(strcnn);
                    //SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                    //DataTable dt = new DataTable();
                    //da.Fill(dt);

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
                customers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            Customers cus = new Customers();
            string Sql = "SELECT * FROM Customers WHERE CustomerID = N'" + id + "'";
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cus.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                cus.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                cus.ContactName = dt.Rows[0]["ContactName"].ToString();
                cus.ContactTitle = dt.Rows[0]["ContactTitle"].ToString();
                cus.Address = dt.Rows[0]["Address"].ToString();
                cus.City = dt.Rows[0]["City"].ToString();
                cus.Region = dt.Rows[0]["Region"].ToString();
                cus.PostalCode = dt.Rows[0]["PostalCode"].ToString();
                cus.Country = dt.Rows[0]["Country"].ToString();
                cus.Phone = dt.Rows[0]["Phone"].ToString();
                cus.Fax = dt.Rows[0]["Fax"].ToString();
            }
            customers.Clear();
            return View(cus);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Customers obj)
        {
            try
            {
                // TODO: Add update logic here
                if (obj != null) 
                {
                    string Sql = "UPDATE Customers SET CompanyName=N'" + obj.CompanyName + "', " +
                        "ContactName=N'" + obj.ContactName + "', " +
                        "ContactTitle=N'" + obj.ContactTitle + "', " +
                        "Address=N'" + obj.Address + "', " +
                        "City=N'" + obj.City + "', " +
                        "Region=N'" + obj.Region + "', " +
                        "PostalCode=N'" + obj.PostalCode + "', " +
                        "Country=N'" + obj.Country + "', " +
                        "Phone=N'" + obj.Phone + "', " +
                        "Fax=N'" + obj.Fax + "' " +
                        "WHERE CustomerID=N'" + id + "'";
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
                customers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            Customers cus = new Customers();
            string Sql = "SELECT * FROM Customers WHERE CustomerID=N'" + id + "'";
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cus.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                cus.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                cus.ContactName = dt.Rows[0]["ContactName"].ToString();
                cus.ContactTitle = dt.Rows[0]["ContactTitle"].ToString();
                cus.Address = dt.Rows[0]["Address"].ToString();
                cus.City = dt.Rows[0]["City"].ToString();
                cus.Region = dt.Rows[0]["Region"].ToString();
                cus.PostalCode = dt.Rows[0]["PostalCode"].ToString();
                cus.Country = dt.Rows[0]["Country"].ToString();
                cus.Phone = dt.Rows[0]["Phone"].ToString();
                cus.Fax = dt.Rows[0]["Fax"].ToString();
            }
            customers.Clear();
            return View(cus);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Customers obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null)
                {
                    string Sql = "DELETE FROM Customers WHERE CustomerID=N'" + id + "'";
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
                customers.Clear();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
