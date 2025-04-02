using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Homework.Controllers
{
    public class SuppliersController : Controller
    {
        string strcnn = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        static List<Suppliers> suppliers = new List<Suppliers>();

        // GET: Suppliers
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            string Sql = "SELECT * FROM Suppliers";
            da = new SqlDataAdapter(Sql, conn);
            da.Fill(ds, "Suppliers");

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Suppliers supp = new Suppliers();
                supp.SupplierID = int.Parse(item["SupplierID"].ToString());
                supp.CompanyName = item["CompanyName"].ToString();
                supp.ContactName = item["ContactName"].ToString();
                supp.ContactTitle = item["ContactTitle"].ToString();
                supp.Address = item["Address"].ToString();
                supp.City = item["City"].ToString();
                supp.Region = item["Region"].ToString();
                supp.PostalCode = item["PostalCode"].ToString();
                supp.Country = item["Country"].ToString();
                supp.Phone = item["Phone"].ToString();
                supp.Fax = item["Fax"].ToString();
                supp.Homepage = item["HomePage"].ToString();
                suppliers.Add(supp);
            }
            return View(suppliers);
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int id)
        {
            Suppliers supp = new Suppliers();
            string Sql = "SELECT * FROM Suppliers WHERE SupplierID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                supp.SupplierID = (int)dt.Rows[0]["SupplierID"];
                supp.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                supp.ContactName = dt.Rows[0]["ContactName"].ToString();
                supp.ContactTitle = dt.Rows[0]["ContactTitle"].ToString();
                supp.Address = dt.Rows[0]["Address"].ToString();
                supp.City = dt.Rows[0]["City"].ToString();
                supp.Region = dt.Rows[0]["Region"].ToString();
                supp.PostalCode = dt.Rows[0]["PostalCode"].ToString();
                supp.Country = dt.Rows[0]["Country"].ToString();
                supp.Phone = dt.Rows[0]["Phone"].ToString();
                supp.Fax = dt.Rows[0]["Fax"].ToString();
                supp.Homepage = dt.Rows[0]["HomePage"].ToString();
            }
            suppliers.Clear();
            return View(supp);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        public ActionResult Create(Suppliers obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj != null)
                {
                    string Sql = "INSERT INTO Suppliers (CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax, HomePage) " +
                        "Values(N'" + obj.CompanyName + "'," +
                        " N'" + obj.ContactName + "'," +
                        " N'" + obj.ContactTitle + "'," +
                        " N'" + obj.Address + "'," +
                        " N'" + obj.City + "'," +
                        " N'" + obj.Region + "'," +
                        " N'" + obj.PostalCode + "'," +
                        " N'" + obj.Country + "'," +
                        " N'" + obj.Phone + "'," +
                        " N'" + obj.Fax + "'," +
                        " N'" + obj.Homepage + "')";
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
                suppliers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int id)
        {
            Suppliers supp = new Suppliers();
            string Sql = "SELECT * FROM Suppliers WHERE SupplierID =" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                supp.SupplierID = (int)dt.Rows[0]["SupplierID"];
                supp.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                supp.ContactName = dt.Rows[0]["ContactName"].ToString();
                supp.ContactTitle = dt.Rows[0]["ContactTitle"].ToString();
                supp.Address = dt.Rows[0]["Address"].ToString();
                supp.City = dt.Rows[0]["City"].ToString();
                supp.Region = dt.Rows[0]["Region"].ToString();
                supp.PostalCode = dt.Rows[0]["PostalCode"].ToString();
                supp.Country = dt.Rows[0]["Country"].ToString();
                supp.Phone = dt.Rows[0]["Phone"].ToString();
                supp.Fax = dt.Rows[0]["Fax"].ToString();
                supp.Homepage = dt.Rows[0]["HomePage"].ToString();
            }
            suppliers.Clear();
            return View(supp);
        }

        // POST: Suppliers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Suppliers obj)
        {
            try
            {
                // TODO: Add update logic here
                if (obj != null)
                {
                    string Sql = "UPDATE Suppliers SET CompanyName=N'" + obj.CompanyName + "', " +
                        "ContactName=N'" + obj.ContactName + "', " +
                        "ContactTitle=N'" + obj.ContactTitle + "', " +
                        "Address=N'" + obj.Address + "', " +
                        "City=N'" + obj.City + "', " +
                        "Region=N'" + obj.Region + "', " +
                        "PostalCode=N'" + obj.PostalCode + "', " +
                        "Country=N'" + obj.Country + "', " +
                        "Phone=N'" + obj.Phone + "', " +
                        "Fax=N'" + obj.Fax + "', " +
                        "HomePage=N'" + obj.Homepage + "' " +
                        "WHERE SupplierID=" + id;
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
                suppliers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int id)
        {
            Suppliers supp = new Suppliers();
            string Sql = "SELECT * FROM Suppliers WHERE SupplierID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                supp.SupplierID = (int)dt.Rows[0]["SupplierID"];
                supp.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                supp.ContactName = dt.Rows[0]["ContactName"].ToString();
                supp.ContactTitle = dt.Rows[0]["ContactTitle"].ToString();
                supp.Address = dt.Rows[0]["Address"].ToString();
                supp.City = dt.Rows[0]["City"].ToString();
                supp.Region = dt.Rows[0]["Region"].ToString();
                supp.PostalCode = dt.Rows[0]["PostalCode"].ToString();
                supp.Country = dt.Rows[0]["Country"].ToString();
                supp.Phone = dt.Rows[0]["Phone"].ToString();
                supp.Fax = dt.Rows[0]["Fax"].ToString();
                supp.Homepage = dt.Rows[0]["HomePage"].ToString();
            }
            suppliers.Clear();
            return View(supp);
        }

        // POST: Suppliers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Suppliers obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null)
                {
                    string Sql = "DELETE FROM Suppliers WHERE SupplierID=" + id;
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
                suppliers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
