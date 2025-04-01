using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;

namespace Homework.Controllers
{
    public class ShippersController : Controller
    {
        static string strcnn = "Server=DESKTOP-A5P4OVC\\SQLEXPRESS;Database=Northwind;Integrated Security=True;";
        static List<Shippers> shippers = new List<Shippers>();
        // GET: Shippers
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string Sql = "SELECT * FROM Shippers";
            da = new SqlDataAdapter(Sql, conn);
            da.Fill(ds, "Shippers");

            foreach (DataRow item in ds.Tables[0].Rows) 
            {
                Shippers shipper = new Shippers();
                shipper.ShipperID = int.Parse(item["ShipperID"].ToString());
                shipper.CompanyName = item["CompanyName"].ToString();
                shipper.Phone = item["Phone"].ToString();
                shippers.Add(shipper);
            }
            return View(shippers);
        }

        // GET: Shippers/Details/5
        public ActionResult Details(int id)
        {
            Shippers shipper = new Shippers();
            string Sql = "SELECT * FROM Shippers WHERE ShipperID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count>0)
            {
                shipper.ShipperID = (int)dt.Rows[0]["ShipperID"];
                shipper.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                shipper.Phone = dt.Rows[0]["Phone"].ToString();
            }
            shippers.Clear();
            return View(shipper);
        }

        // GET: Shippers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shippers/Create
        [HttpPost]
        public ActionResult Create(Shippers obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj != null)
                {
                    string Sql = "INSERT INTO Shippers (CompanyName, Phone) VALUES (N'" + obj.CompanyName + "' , N'" + obj.Phone + "')";
                    //string Sql = "SELECT MAX(ShipperID) FROM Shippers ";
                    SqlConnection conn = new SqlConnection(strcnn);
                    SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    //int ShipperID = 1;
                    //if (dt.Rows.Count>0)
                    //{
                    //    ShipperID = (int)dt.Rows[0][0] + 1;
                    //}

                    //if (conn.State == ConnectionState.Closed)
                    //{
                    //    conn.Open();
                    //}
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.Connection = conn;
                    //cmd.CommandText = Sql;
                    //cmd.CommandType = CommandType.Text;
                    //cmd.ExecuteNonQuery();
                }
                shippers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shippers/Edit/5
        public ActionResult Edit(int id)
        {
            Shippers shipper = new Shippers();
            string Sql = "SELECT * FROM Shippers WHERE ShipperID =" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count>0)
            {
                shipper.ShipperID = (int)dt.Rows[0]["ShipperID"];
                shipper.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                shipper.Phone = dt.Rows[0]["Phone"].ToString();
            }
            return View(shipper);
        }

        // POST: Shippers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Shippers obj)
        {
            try
            {
                // TODO: Add update logic here
                if (obj != null)
                {
                    string Sql = "UPDATE Shippers SET CompanyName=N'" + obj.CompanyName + "', Phone=N'" + obj.Phone + "' WHERE ShipperID=" + id;
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
                shippers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Shippers/Delete/5
        public ActionResult Delete(int id)
        {
            Shippers shipper = new Shippers();
            string Sql = "SELECT * FROM Shippers WHERE ShipperID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count>0)
            {
                shipper.ShipperID = (int)dt.Rows[0]["ShipperID"];
                shipper.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                shipper.Phone = dt.Rows[0]["Phone"].ToString();
            }
            shippers.Clear();
            return View(shipper);
        }

        // POST: Shippers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Shippers obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null)
                {
                    string Sql = "DELETE FROM Shippers WHERE ShipperID=" + id;
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
                shippers.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
