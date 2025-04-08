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
    public class TerritoriesController : BaseController
    {
        string strcnn = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        List<Territories> territories = new List<Territories>();
        // GET: Territories
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            string Sql = "SELECT * FROM Territories";
            da = new SqlDataAdapter(Sql, conn);
            da.Fill(ds, "Territories");

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Territories ter = new Territories();
                ter.TerritoryID = item["TerritoryID"].ToString();
                ter.TerritoryDescription = item["TerritoryDescription"].ToString();
                ter.RegionID = int.Parse(item["RegionID"].ToString());
                territories.Add(ter);
            }
            return View(territories);
        }

        // GET: Territories/Details/5
        public ActionResult Details(string id)
        {
            Territories ter = new Territories();
            string Sql = "SELECT * FROM Territories WHERE TerritoryID=N'" + id + "'";
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                ter.TerritoryID = dt.Rows[0]["TerritoryID"].ToString();
                ter.TerritoryDescription = dt.Rows[0]["TerritoryDescription"].ToString();
                ter.RegionID = (int)dt.Rows[0]["RegionID"];
            }
            territories.Clear();
            return View(ter);
        }

        // GET: Territories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Territories/Create
        [HttpPost]
        public ActionResult Create(Territories obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj != null) 
                {
                    string Sql = "INSERT INTO Territories (TerritoryID, TerritoryDescription, RegionID) " +
                        "Values(N'" + obj.TerritoryID + "'," +
                        "N'" + obj.TerritoryDescription + "'," +
                        "" + obj.RegionID + ")";
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
                territories.Clear ();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Territories/Edit/5
        public ActionResult Edit(string id)
        {
            Territories ter = new Territories();
            string Sql = "SELECT * FROM Territories WHERE TerritoryID=N'" + id + "'";
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                ter.TerritoryID = dt.Rows[0]["TerritoryID"].ToString();
                ter.TerritoryDescription = dt.Rows[0]["TerritoryDescription"].ToString();
                ter.RegionID = (int)dt.Rows[0]["RegionID"];
            }
            territories.Clear();
            return View(ter);
        }

        // POST: Territories/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Territories obj)
        {
            try
            {
                // TODO: Add update logic here
                if (obj != null) 
                {
                    string Sql = "UPDATE Territories " +
                        "SET TerritoryDesciption=N'" + obj.TerritoryDescription + "', " +
                        "RegionID= " + obj.RegionID + " " +
                        "WHERE TerritoryID=N'" + obj.TerritoryID + "'";
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
                territories.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Territories/Delete/5
        public ActionResult Delete(string id)
        {
            Territories ter = new Territories();
            string Sql = "SELECT * FROM Territories WHERE TerritoryID=N'" + id + "'";
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                ter.TerritoryID = dt.Rows[0]["TerritoryID"].ToString();
                ter.TerritoryDescription = dt.Rows[0]["TerritoryDescription"].ToString();
                ter.RegionID = (int)dt.Rows[0]["RegionID"];
            }
            territories.Clear();
            return View(ter);
        }

        // POST: Territories/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, Territories obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null) 
                {
                    string Sql = "DELETE FROM Territories WHERE TerritoryID=N'" + id + "'";
                    SqlConnection conn = new SqlConnection(strcnn);
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = Sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                territories.Clear();
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
