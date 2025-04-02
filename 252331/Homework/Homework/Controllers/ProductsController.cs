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
    public class ProductsController : Controller
    {
        string strcnn = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        List<Products> products = new List<Products>();
        // GET: Products
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter();

            DataSet ds = new DataSet();
            string Sql = "SELECT * FROM Products";
            da = new SqlDataAdapter(Sql, conn);
            da.Fill(ds, "Products");

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Products product = new Products();
                product.ProductID = int.Parse(item["ProductID"].ToString());
                product.ProductName = item["ProductName"].ToString();
                product.SupplierID = int.Parse(item["SupplierID"].ToString());
                product.CategoryID = int.Parse(item["CategoryID"].ToString());
                product.QuantityPerUnit = item["QuantityPerUnit"].ToString();
                product.UnitPrice = int.Parse(item["UnitPrice"].ToString());
                product.UnitsInStock = int.Parse(item["UnitsInStock"].ToString());
                product.UnitsOnOrder = int.Parse(item["UnitsOnOrder"].ToString());
                product.ReorderLevel = int.Parse(item["ReorderLevel"].ToString());
                product.Discontinued = bool.Parse(item["ProductName"].ToString());
                products.Add(product);
            }
            products.Clear();

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            Products product = new Products();
            string Sql = "SELECT * FROM Products WHERE ProductID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                product.ProductID = (int)dt.Rows[0]["ProductID"];
                product.ProductName = dt.Rows[0]["ProductName"].ToString();
                product.SupplierID = (int)dt.Rows[0]["SupplierID"];
                product.CategoryID = (int)dt.Rows[0]["CategoryID"];
                product.QuantityPerUnit = dt.Rows[0]["QuantityPerUnit"].ToString();
                product.UnitPrice = (int)dt.Rows[0]["UnitPrice"];
                product.UnitsInStock = (int)dt.Rows[0]["UnitsInStock"];
                product.UnitsOnOrder = (int)dt.Rows[0]["UnitsOnOrder"];
                product.ReorderLevel = (int)dt.Rows[0]["ReorderLevel"];
                product.Discontinued = (bool)dt.Rows[0]["Discontinued"];
            }
            products.Clear();

            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(Products obj)
        {
            try
            {
                // TODO: Add insert logic here
                if (obj != null)
                {
                    string Sql = "INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued) " +
                        "Values(N'" + obj.ProductName + "', " +
                        ""+ obj.SupplierID +", " +
                        ""+ obj.CategoryID +", " +
                        "N'"+ obj.QuantityPerUnit +"', " +
                        ""+ obj.UnitPrice +", " +
                        ""+ obj.UnitsInStock +", " +
                        ""+ obj.UnitsOnOrder +", " +
                        ""+ obj.ReorderLevel +", " +
                        ""+ obj.Discontinued +")";
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
                products.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            Products product = new Products();
            string Sql = "SELECT * FROM Products WHERE ProductID =" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                product.ProductID = (int)dt.Rows[0]["ProductID"];
                product.ProductName = dt.Rows[0]["ProductName"].ToString();
                product.SupplierID = (int)dt.Rows[0]["SupplierID"];
                product.CategoryID = (int)dt.Rows[0]["CategoryID"];
                product.QuantityPerUnit = dt.Rows[0]["QuantityPerUnit"].ToString();
                product.UnitPrice = (int)dt.Rows[0]["UnitPrice"];
                product.UnitsInStock = (int)dt.Rows[0]["UnitsInStock"];
                product.UnitsOnOrder = (int)dt.Rows[0]["UnitsOnOrder"];
                product.ReorderLevel = (int)dt.Rows[0]["ReorderLevel"];
                product.Discontinued = (bool)dt.Rows[0]["Discontinued"];
            }
            products.Clear();
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Products obj)
        {
            try
            {
                // TODO: Add update logic here
                if (obj != null)
                {
                    string Sql = "UPDATE Products " +
                        "SET ProductName=N'" + obj.ProductName + "', " +
                        "SupplierID=" + obj.SupplierID + ", " +
                        "CategoryID=" + obj.CategoryID + ", " +
                        "QuantityPerUnit=N'" + obj.QuantityPerUnit + "', " +
                        "UnitPrice=" + obj.UnitPrice + ", " +
                        "UnitsInStock=" + obj.UnitsInStock + ", " +
                        "UnitsOnOrder=" + obj.UnitsOnOrder + ", " +
                        "ReorderLevel=" + obj.ReorderLevel + ", " +
                        "Discontinued=" + obj.Discontinued + 
                        " WHERE ProductID=" + id;
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
                products.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            Products product = new Products();
            string Sql = "SELECT * FROM Products WHERE ProductID=" + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                product.ProductID = (int)dt.Rows[0]["ProductID"];
                product.ProductName = dt.Rows[0]["ProductName"].ToString();
                product.SupplierID = (int)dt.Rows[0]["SupplierID"];
                product.CategoryID = (int)dt.Rows[0]["CategoryID"];
                product.QuantityPerUnit = dt.Rows[0]["QuantityPerUnit"].ToString();
                product.UnitPrice = (int)dt.Rows[0]["UnitPrice"];
                product.UnitsInStock = (int)dt.Rows[0]["UnitsInStock"];
                product.UnitsOnOrder = (int)dt.Rows[0]["UnitsOnOrder"];
                product.ReorderLevel = (int)dt.Rows[0]["ReorderLevel"];
                product.Discontinued = (bool)dt.Rows[0]["Discontinued"];
            }
            products.Clear();
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Products obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null)
                {
                    string Sql = "DELETE FROM Products WHERE ProductID=" + id;
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
                products.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
