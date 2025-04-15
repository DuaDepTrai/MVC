using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework.Models;
using System.Diagnostics;
using System.Globalization;


namespace Homework.Controllers
{
    public class OrdersController : Controller
    {
        string strcnn = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        static List<Orders> orders = new List<Orders>();
        // GET: Orders
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(strcnn);

            string Sql = "SELECT Orders.OrderID, " +
                "Orders.CustomerID, " +
                "Orders.EmployeeID, " +
                "Orders.OrderDate, " +
                "Orders.RequiredDate, " +
                "Orders.ShippedDate, " +
                "Orders.ShipVia, " +
                "Orders.Freight, " +
                "Orders.ShipName, " +
                "Orders.ShipAddress, " +
                "Orders.ShipCity, " +
                "Orders.ShipRegion, " +
                "Orders.ShipPostalCode, " +
                "Orders.ShipCountry, " +
                "Customers.CompanyName AS CustomerName, " +
                "Shippers.CompanyName AS ShipperName, " +
                "Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM Orders " +
                "LEFT OUTER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID " +
                "LEFT OUTER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID " +
                "LEFT OUTER JOIN Customers ON Orders.CustomerID = Customers.CustomerID " +
                "ORDER BY Orders.OrderID DESC";

            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataSet ds = new DataSet(); 
            da.Fill(ds, "Orders");

            orders.Clear();

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                Orders order = new Orders();
                order.OrderID = int.Parse(item["OrderID"].ToString());
                order.CustomerID = item["CustomerID"].ToString();
                order.CustomerName = item["CustomerName"].ToString();
                order.EmployeeID = int.Parse(item["EmployeeID"].ToString());
                order.EmployeeName = item["FullName"].ToString();
                order.OrderDate = (DateTime) item["OrderDate"];
                order.RequiredDate = (DateTime) item["RequiredDate"];
                if (item["ShippedDate"].ToString() != "" && item["ShippedDate"] != null)
                {
                    order.ShippedDate = (DateTime)item["ShippedDate"];
                }
                order.ShipVia = int.Parse(item["ShipVia"].ToString());
                order.ShipperName = item["ShipperName"].ToString();
                order.Freight = decimal.Parse(item["Freight"].ToString());
                order.ShipName = item["ShipName"].ToString();
                order.ShipAddress = item["ShipAddress"].ToString();
                order.ShipCity = item["ShipCity"].ToString();
                order.ShipRegion = item["ShipRegion"].ToString();
                order.ShipPostalCode = item["ShipPostalCode"].ToString();
                order.ShipCountry = item["ShipCountry"].ToString();
                orders.Add(order);
            }

            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            Orders order = new Orders();
            string Sql = "SELECT Orders.OrderID, " +
                "Orders.CustomerID, " +
                "Orders.EmployeeID, " +
                "Orders.OrderDate, " +
                "Orders.RequiredDate, " +
                "Orders.ShippedDate, " +
                "Orders.ShipVia, " +
                "Orders.Freight, " +
                "Orders.ShipName, " +
                "Orders.ShipAddress, " +
                "Orders.ShipCity, " +
                "Orders.ShipRegion, " +
                "Orders.ShipPostalCode, " +
                "Orders.ShipCountry, " +
                "Customers.CompanyName AS CustomerName, " +
                "Shippers.CompanyName AS ShipperName, " +
                "Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM Orders " +
                "LEFT OUTER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID " +
                "LEFT OUTER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID " +
                "LEFT OUTER JOIN Customers ON Orders.CustomerID = Customers.CustomerID " +
                "WHERE Orders.OrderID = " + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                order.OrderID = int.Parse(dt.Rows[0]["OrderID"].ToString());
                order.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                order.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                order.EmployeeID = int.Parse(dt.Rows[0]["EmployeeID"].ToString());
                order.EmployeeName = dt.Rows[0]["FullName"].ToString();
                order.OrderDate = (DateTime)dt.Rows[0]["OrderDate"];
                order.RequiredDate = (DateTime)dt.Rows[0]["RequiredDate"];
                if (dt.Rows[0]["ShippedDate"].ToString() != "" && dt.Rows[0]["ShippedDate"] != null)
                {
                    order.ShippedDate = (DateTime)dt.Rows[0]["ShippedDate"];
                }
                order.ShipVia = int.Parse(dt.Rows[0]["ShipVia"].ToString());
                order.ShipperName = dt.Rows[0]["ShipperName"].ToString();
                order.Freight = decimal.Parse(dt.Rows[0]["Freight"].ToString());
                order.ShipName = dt.Rows[0]["ShipName"].ToString();
                order.ShipAddress = dt.Rows[0]["ShipAddress"].ToString();
                order.ShipCity = dt.Rows[0]["ShipCity"].ToString();
                order.ShipRegion = dt.Rows[0]["ShipRegion"].ToString();
                order.ShipPostalCode = dt.Rows[0]["ShipPostalCode"].ToString();
                order.ShipCountry = dt.Rows[0]["ShipCountry"].ToString();
            }
            orders.Clear();

            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            LoadCustomers();
            LoadEmployees();
            LoadShippers();

            List<OrderDetails> listOrders = new List<OrderDetails>();

            if (Session["listOrders"] == null)
            {
                Session["listOrders"] = listOrders;
            }


            return View(new Orders());
        }

        // POST: Orders/Create
        [HttpPost]
        public ActionResult Create(Orders obj)
        {
            if (!ModelState.IsValid)
            {
                LoadCustomers();
                LoadEmployees();
                LoadShippers();
                return View(obj);
            }
            try
            {
                // TODO: Add insert logic here
                if (obj != null)
                {
                    string Sql = "INSERT INTO Orders (CustomerID, " +
                        "EmployeeID, OrderDate, RequiredDate, ShippedDate, " +
                        "ShipVia, Freight, ShipName, ShipAddress, ShipCity, " +
                        "ShipRegion, ShipPostalCode, ShipCountry) " +
                        "Values(N'" + obj.CustomerID + "', " +
                        "" + obj.EmployeeID + ", " +
                        "'" + obj.OrderDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        "'" + obj.RequiredDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        "'" + obj.ShippedDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        "" + obj.ShipVia + ", " +
                        "" + obj.Freight + ", " +
                        "N'" + obj.ShipName + "', " +
                        "N'" + obj.ShipAddress + "', " +
                        "N'" + obj.ShipCity + "', " +
                        "N'" + obj.ShipRegion + "', " +
                        "N'" + obj.ShipPostalCode + "', " +
                        "N'" + obj.ShipCountry + "')";
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

                    //Order Details
                    Sql = "SELECT Max(OrderID) FROM Orders";
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
                    da.Fill(dt);

                    var OrderID = dt.Rows[0][0];

                    List<OrderDetails> orderDetails = new List<OrderDetails>();
                    int i = 0;
                    if (Session["listOrders"] != null)
                    {
                        orderDetails = (List<OrderDetails>)Session["listOrders"];
                    }

                    foreach (var item in orderDetails) 
                    {
                        Sql = "INSERT INTO [Order Details] (OrderID, " +
                        "ProductID, UnitPrice, Quantity, Discount) " +
                        "Values(" + OrderID + "', " +
                        "" + item.ProductID + ", " +
                        "'" + item.UnitPrice + "', " +
                        "'" + item.Quantity + "', " +
                        "'" + item.Discount + "')";
                        cmd.CommandText = Sql;
                        cmd.ExecuteNonQuery();
                    }

                }
                orders.Clear();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);
                LoadCustomers();
                LoadEmployees();
                LoadShippers();
                return View();
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            LoadCustomers();
            LoadEmployees();
            LoadShippers();

            Orders order = new Orders();
            string Sql = "SELECT Orders.OrderID, " +
                "Orders.CustomerID, " +
                "Orders.EmployeeID, " +
                "Orders.OrderDate, " +
                "Orders.RequiredDate, " +
                "Orders.ShippedDate, " +
                "Orders.ShipVia, " +
                "Orders.Freight, " +
                "Orders.ShipName, " +
                "Orders.ShipAddress, " +
                "Orders.ShipCity, " +
                "Orders.ShipRegion, " +
                "Orders.ShipPostalCode, " +
                "Orders.ShipCountry, " +
                "Customers.CompanyName AS CustomerName, " +
                "Shippers.CompanyName AS ShipperName, " +
                "Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM Orders " +
                "LEFT OUTER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID " +
                "LEFT OUTER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID " +
                "LEFT OUTER JOIN Customers ON Orders.CustomerID = Customers.CustomerID " +
                "WHERE Orders.OrderID = " + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                order.OrderID = int.Parse(dt.Rows[0]["OrderID"].ToString());
                order.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                order.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                order.EmployeeID = int.Parse(dt.Rows[0]["EmployeeID"].ToString());
                order.EmployeeName = dt.Rows[0]["FullName"].ToString();
                order.OrderDate = (DateTime)dt.Rows[0]["OrderDate"];
                order.RequiredDate = (DateTime)dt.Rows[0]["RequiredDate"];
                if (dt.Rows[0]["ShippedDate"].ToString() != "" && dt.Rows[0]["ShippedDate"] != null)
                {
                    order.ShippedDate = (DateTime)dt.Rows[0]["ShippedDate"];
                }
                order.ShipVia = int.Parse(dt.Rows[0]["ShipVia"].ToString());
                order.ShipperName = dt.Rows[0]["ShipperName"].ToString();
                order.Freight = decimal.Parse(dt.Rows[0]["Freight"].ToString());
                order.ShipName = dt.Rows[0]["ShipName"].ToString();
                order.ShipAddress = dt.Rows[0]["ShipAddress"].ToString();
                order.ShipCity = dt.Rows[0]["ShipCity"].ToString();
                order.ShipRegion = dt.Rows[0]["ShipRegion"].ToString();
                order.ShipPostalCode = dt.Rows[0]["ShipPostalCode"].ToString();
                order.ShipCountry = dt.Rows[0]["ShipCountry"].ToString();
            }
            orders.Clear();

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Orders obj)
        {
            if (!ModelState.IsValid)
            {
                LoadCustomers();
                LoadEmployees();
                LoadShippers();
                return View(obj);
            }
            try
            {
                // TODO: Add update logic here
                if (obj != null)
                {

                    string orderDate = obj.OrderDate == DateTime.MinValue
                        ? "NULL"
                        : "'" + obj.OrderDate.ToString("yyyy-MM-dd") + "'";

                    string requiredDate = obj.RequiredDate == DateTime.MinValue
                        ? "NULL"
                        : "'" + obj.RequiredDate.ToString("yyyy-MM-dd") + "'";

                    string shippedDate = obj.ShippedDate == DateTime.MinValue
                        ? "NULL"
                        : "'" + obj.ShippedDate.ToString("yyyy-MM-dd") + "'";

                    string Sql = "UPDATE Orders " +
                        "SET CustomerID=N'" + obj.CustomerID + "', " +
                        "EmployeeID=" + obj.EmployeeID + ", " +
                        "OrderDate=" + orderDate + ", " +
                        "RequiredDate=" + requiredDate + ", " +
                        "ShippedDate=" + shippedDate + ", " +
                        "ShipVia=" + obj.ShipVia + ", " +
                        "Freight=" + obj.Freight.ToString(CultureInfo.InvariantCulture) + ", " +
                        "ShipName=N'" + obj.ShipName + "', " +
                        "ShipAddress=N'" + obj.ShipAddress + "', " +
                        "ShipCity=N'" + obj.ShipCity + "', " +
                        "ShipRegion=N'" + obj.ShipRegion + "', " +
                        "ShipPostalCode=N'" + obj.ShipPostalCode + "', " +
                        "ShipCountry=N'" + obj.ShipCountry + "' " +
                        "WHERE OrderID=" + id;
                    Debug.WriteLine(Sql);  // hoặc Console.WriteLine nếu chạy console

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
                orders.Clear();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                LoadCustomers();
                LoadEmployees();
                LoadShippers();
                return View();
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            Orders order = new Orders();
            string Sql = "SELECT Orders.OrderID, " +
                "Orders.CustomerID, " +
                "Orders.EmployeeID, " +
                "Orders.OrderDate, " +
                "Orders.RequiredDate, " +
                "Orders.ShippedDate, " +
                "Orders.ShipVia, " +
                "Orders.Freight, " +
                "Orders.ShipName, " +
                "Orders.ShipAddress, " +
                "Orders.ShipCity, " +
                "Orders.ShipRegion, " +
                "Orders.ShipPostalCode, " +
                "Orders.ShipCountry, " +
                "Customers.CompanyName AS CustomerName, " +
                "Shippers.CompanyName AS ShipperName, " +
                "Employees.LastName + ' ' + Employees.FirstName AS FullName " +
                "FROM Orders " +
                "LEFT OUTER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID " +
                "LEFT OUTER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID " +
                "LEFT OUTER JOIN Customers ON Orders.CustomerID = Customers.CustomerID " +
                "WHERE Orders.OrderID = " + id;
            SqlConnection conn = new SqlConnection(strcnn);
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                order.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                order.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                order.EmployeeID = int.Parse(dt.Rows[0]["EmployeeID"].ToString());
                order.EmployeeName = dt.Rows[0]["FullName"].ToString();
                order.OrderDate = (DateTime)dt.Rows[0]["OrderDate"];
                order.RequiredDate = (DateTime)dt.Rows[0]["RequiredDate"];
                if (dt.Rows[0]["ShippedDate"].ToString() != "" && dt.Rows[0]["ShippedDate"] != null)
                {
                    order.ShippedDate = (DateTime)dt.Rows[0]["ShippedDate"];
                }
                order.ShipVia = int.Parse(dt.Rows[0]["ShipVia"].ToString());
                order.ShipperName = dt.Rows[0]["ShipperName"].ToString();
                order.Freight = decimal.Parse(dt.Rows[0]["Freight"].ToString());
                order.ShipName = dt.Rows[0]["ShipName"].ToString();
                order.ShipAddress = dt.Rows[0]["ShipAddress"].ToString();
                order.ShipCity = dt.Rows[0]["ShipCity"].ToString();
                order.ShipRegion = dt.Rows[0]["ShipRegion"].ToString();
                order.ShipPostalCode = dt.Rows[0]["ShipPostalCode"].ToString();
                order.ShipCountry = dt.Rows[0]["ShipCountry"].ToString();
            }
            orders.Clear();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Orders obj)
        {
            try
            {
                // TODO: Add delete logic here
                if (obj != null)
                {
                    string Sql = "DELETE FROM Orders WHERE OrderID=" + id;
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
                orders.Clear();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void LoadCustomers()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT CustomerID, CompanyName FROM Customers";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Customers> cuss = new List<Customers>();
            foreach (DataRow item in dt.Rows)
            {
                Customers cus = new Customers();
                cus.CustomerID = item["CustomerID"].ToString();
                cus.CompanyName = item["CompanyName"].ToString();
                cuss.Add(cus);
            }
            ViewBag.Customers = cuss;
        }

        private void LoadEmployees()
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
        }

        private void LoadShippers()
        {
            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT ShipperID, CompanyName FROM Shippers";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Shippers> shippers = new List<Shippers>();
            foreach (DataRow item in dt.Rows)
            {
                Shippers shipper = new Shippers();
                shipper.ShipperID = int.Parse(item["ShipperID"].ToString());
                shipper.CompanyName = item["CompanyName"].ToString();
                shippers.Add(shipper);
            }
            ViewBag.Shippers = shippers;
        }
    }
}
