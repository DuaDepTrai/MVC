using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace App.Controllers
{
    public class RegionController : Controller
    {
        //string strcnn = "Server=DESKTOP-A5P4OVC\\SQLEXPRESS;Database=Northwind;Integrated Security=True;";
        string strcnn = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        static int ID = 0;
        static List<Region> regions = new List<Region>();
        // GET: Region
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            string Sql = "SELECT * FROM Region";
            da = new SqlDataAdapter(Sql, conn);
            da.Fill(ds, "Region");

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Region region = new Region();
                region.RegionID = int.Parse(item["RegionID"].ToString());
                region.RegionDescription = item["RegionDescription"].ToString();
                regions.Add(region);
            }
            regions.Clear();

            return View(regions);
        }

        // GET: Region/Details/5
        public ActionResult Details(int id)
        {
            Region region = new Region();
            string Sql = "SELECT * FROM Region WHERE RegionID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                region.RegionID = (int)dt.Rows[0]["RegionID"];
                region.RegionDescription = dt.Rows[0]["RegionDescription"].ToString();
            }
            regions.Clear();

            return View(region);
        }

        // GET: Region/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Region/Create
        [HttpPost]
        public ActionResult Create(Region obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj != null)
                {
                    string Sql = "SELECT MAX(RegionID) FROM Region ";
                    SqlConnection conn = new SqlConnection(strcnn);
                    SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    int RegionID = 1;
                    if (dt.Rows.Count > 0)
                    {
                        RegionID = (int)dt.Rows[0][0] + 1;
                    }

                    Sql = "INSERT INTO Region (RegionID, RegionDEscription) Values(" + RegionID + ",N'" + obj.RegionDescription + "' )";
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
                regions.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Region/Edit/5
        public ActionResult Edit(int id)
        {
            Region region = new Region();
            string Sql = "SELECT * FROM Region WHERE RegionID =" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                region.RegionID = (int)dt.Rows[0]["RegionID"];
                region.RegionDescription = dt.Rows[0]["RegionDescription"].ToString();
            }
            regions.Clear();

            return View(region);
        }

        // POST: Region/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Region obj)
        {
            try
            {
                // TODO: Add update logic here
                if (obj != null)
                {
                    string Sql = "UPDATE Region SET RegionDescription=N'" + obj.RegionDescription + "' WHERE RegionID=" + id;
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
                regions.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Region/Delete/5
        public ActionResult Delete(int id)
        {
            Region region = new Region();
            string Sql = "SELECT * FROM Region WHERE RegionID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                region.RegionID = (int)dt.Rows[0]["RegionID"];
                region.RegionDescription = dt.Rows[0]["RegionDescription"].ToString();
            }
            regions.Clear();

            return View(region);
        }

        // POST: Region/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Region obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null)
                {
                    string Sql = "DELETE FROM Region WHERE RegionID=" + id;
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
                regions.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
