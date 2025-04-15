using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FistAppMVC.Models;
using System.Drawing;

namespace FistAppMVC.Controllers
{
    public class OrdersController : Controller
    {
        string strcnn = ConfigurationManager.ConnectionStrings["Chuoiketnoi"].ConnectionString;
        // GET: Orders
        public ActionResult Index()
        {
            List<Orders> orders = new List<Orders>();

            SqlConnection conn = new SqlConnection(strcnn);
            string Sql = "SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, Customers.CompanyName AS CustomerName, Employees.LastName + ' ' + Employees.FirstName AS EmployeeName, Shippers.CompanyName FROM     Orders LEFT OUTER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID LEFT OUTER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID LEFT OUTER JOIN Customers ON Orders.CustomerID = Customers.CustomerID";
            SqlDataAdapter da = new SqlDataAdapter(Sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow item in dt.Rows)
            {
                Orders order = new Orders();
                order.OrderID = int.Parse(item["OrderID"].ToString());
                order.CustomerID = item["CustomerID"].ToString();
                order.CustomerName = item["CustomerName"].ToString();
                order.EmployeeID = int.Parse(item["EmployeeID"].ToString());
                order.EmployeeName = item["EmployeeName"].ToString();
                order.OrderDate = (DateTime)item["OrderDate"];
                order.RequiredDate = (DateTime)item["RequiredDate"];

                if (item["ShippedDate"].ToString() != "" && item["ShippedDate"] != null)
                {
                    order.ShippedDate = Convert.ToDateTime(item["ShippedDate"]);
                }

                order.ShipVia = int.Parse(item["ShipVia"].ToString());
                order.CompanyName = item["CompanyName"].ToString();
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
            return View();
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            Session["ViTri"] = "Create";

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

            Sql = "SELECT * FROM Customers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Customers> listcus = new List<Customers>();
            foreach (DataRow dr in dt.Rows)
            {
                Customers cus = new Customers();
                cus.CustomerID = dr["CustomerID"].ToString();
                cus.CompanyName = dr["CompanyName"].ToString();
                listcus.Add(cus);
            }
            ViewBag.Customers = listcus;

            Sql = "SELECT * FROM Shippers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Shippers> listship = new List<Shippers>();
            foreach (DataRow dr in dt.Rows)
            {
                Shippers ship = new Shippers();
                ship.ShipperID = int.Parse( dr["ShipperID"].ToString());
                ship.CompanyName = dr["CompanyName"].ToString();
                listship.Add(ship);
            }
            ViewBag.Shippers = listship;


            List <OrderDetails> listorders = new List<OrderDetails>();

            if (Session["listorders"]==null)
            { 
                Session["listorders"] = listorders;
            }    
            


            Orders obj = new Orders();
            obj.OrderID = 0;
            return View(obj);
        }

        // POST: Orders/Create
        [HttpPost]
        public ActionResult Create(Orders obj)
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

            Sql = "SELECT * FROM Customers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Customers> listcus = new List<Customers>();
            foreach (DataRow dr in dt.Rows)
            {
                Customers cus = new Customers();
                cus.CustomerID = dr["CustomerID"].ToString();
                cus.CompanyName = dr["CompanyName"].ToString();
                listcus.Add(cus);
            }
            ViewBag.Customers = listcus;

            Sql = "SELECT * FROM Shippers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Shippers> listship = new List<Shippers>();
            foreach (DataRow dr in dt.Rows)
            {
                Shippers ship = new Shippers();
                ship.ShipperID = int.Parse(dr["ShipperID"].ToString());
                ship.CompanyName = dr["CompanyName"].ToString();
                listship.Add(ship);
            }
            ViewBag.Shippers = listship;

            if (ModelState.IsValid) 
            {
                try
                {
                    Sql = "INSERT INTO  Orders (CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry) VALUES (N'" + obj.CustomerID + "'," + obj.EmployeeID + ", '"+ obj.OrderDate.ToString("MM/dd/yyyy") + "','"+ obj.RequiredDate.ToString("MM/dd/yyyy") + "','"+ obj.ShippedDate.ToString("MM/dd/yyyy") + "', "+obj.ShipVia+","+obj.Freight+",N'"+obj.ShipName + "',N'"+obj.ShipAddress+ "',N'"+obj.ShipCity + "' ,N'"+obj.ShipRegion + "',N'"+obj.ShipPostalCode + "' ,N'"+obj.ShipCountry + "') ";
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = Sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    // TODO: Add insert logic here

                    Sql = "SELECT Max(OrderID) FROM Orders";

                    dt = new DataTable();
                    da.Fill(dt);

                    var OrderID = dt.Rows[0][0];

                    List<OrderDetails> orderDetails = new List<OrderDetails>();
                    int i = 0;
                    if (Session["listorders"] != null)
                    {     
                        orderDetails = (List<OrderDetails>)Session["listorders"];
                    }
                    foreach (var item in orderDetails)
                    {
                        Sql = "INSERT INTO  [Order Details] (OrderID,ProductID,UnitPrice,Quantity,Discount) VALUES (" + OrderID + "," + item.ProductID + ", " + item.UnitPrice + "," + item.Quantity + ", " + item.Discount + " ";
                        cmd.CommandText = Sql;
                        cmd.ExecuteNonQuery();
                    }



                    return RedirectToAction("Index");
                }
                catch
                {

                    return View(obj);
                }

                
            }
            return View(obj);

        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int id)
        {
            Session["ViTri"] = "Edit";

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

            Sql = "SELECT * FROM Customers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Customers> listcus = new List<Customers>();
            foreach (DataRow dr in dt.Rows)
            {
                Customers cus = new Customers();
                cus.CustomerID = dr["CustomerID"].ToString();
                cus.CompanyName = dr["CompanyName"].ToString();
                listcus.Add(cus);
            }
            ViewBag.Customers = listcus;

            Sql = "SELECT * FROM Shippers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Shippers> listship = new List<Shippers>();
            foreach (DataRow dr in dt.Rows)
            {
                Shippers ship = new Shippers();
                ship.ShipperID = int.Parse(dr["ShipperID"].ToString());
                ship.CompanyName = dr["CompanyName"].ToString();
                listship.Add(ship);
            }
            ViewBag.Shippers = listship;


            Sql = "SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, Customers.CompanyName AS CustomerName, Employees.LastName + ' ' + Employees.FirstName AS EmployeeName, Shippers.CompanyName FROM     Orders LEFT OUTER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID LEFT OUTER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID LEFT OUTER JOIN Customers ON Orders.CustomerID = Customers.CustomerID WHERE OrderID=" + id;
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            Orders order = new Orders();
            if (dt.Rows.Count>0)
            {
                order.OrderID = int.Parse(dt.Rows[0]["OrderID"].ToString());
                order.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                order.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                order.EmployeeID = int.Parse(dt.Rows[0]["EmployeeID"].ToString());
                order.EmployeeName = dt.Rows[0]["EmployeeName"].ToString();
                order.OrderDate = (DateTime)dt.Rows[0]["OrderDate"];
                order.RequiredDate = (DateTime)dt.Rows[0]["RequiredDate"];

                if (dt.Rows[0]["ShippedDate"].ToString() != "" && dt.Rows[0]["ShippedDate"] != null)
                {
                    order.ShippedDate = Convert.ToDateTime(dt.Rows[0]["ShippedDate"]);
                }

                order.ShipVia = int.Parse(dt.Rows[0]["ShipVia"].ToString());
                order.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                order.Freight = decimal.Parse(dt.Rows[0]["Freight"].ToString());
                order.ShipName = dt.Rows[0]["ShipName"].ToString();
                order.ShipAddress = dt.Rows[0]["ShipAddress"].ToString();
                order.ShipCity = dt.Rows[0]["ShipCity"].ToString();
                order.ShipRegion = dt.Rows[0]["ShipRegion"].ToString();
                order.ShipPostalCode = dt.Rows[0]["ShipPostalCode"].ToString();
                order.ShipCountry = dt.Rows[0]["ShipCountry"].ToString();
            }

            List<OrderDetails> listorders = new List<OrderDetails>();


            Sql = "SELECT [Order Details].OrderID, [Order Details].ProductID, [Order Details].UnitPrice, [Order Details].Quantity, [Order Details].Discount, Products.ProductName FROM     [Order Details] INNER JOIN Products ON [Order Details].ProductID = Products.ProductID WHERE OrderID=" + id;
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                OrderDetails details = new OrderDetails();
                details.OrderID= int.Parse(item["OrderID"].ToString());
                details.ProductID = int.Parse(item["ProductID"].ToString());
                details.ProductName = item["ProductName"].ToString();
                details.UnitPrice = float.Parse(item["UnitPrice"].ToString());
                details.Quantity = int.Parse(item["Quantity"].ToString());
                details.Discount = Decimal.Parse(item["Discount"].ToString());
                listorders.Add(details);
            }


           
            Session["listorders"] = listorders;
            



            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Orders obj)
        {
            Session["ViTri"] = "Edit";

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

            Sql = "SELECT * FROM Customers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Customers> listcus = new List<Customers>();
            foreach (DataRow dr in dt.Rows)
            {
                Customers cus = new Customers();
                cus.CustomerID = dr["CustomerID"].ToString();
                cus.CompanyName = dr["CompanyName"].ToString();
                listcus.Add(cus);
            }
            ViewBag.Customers = listcus;

            Sql = "SELECT * FROM Shippers";
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            List<Shippers> listship = new List<Shippers>();
            foreach (DataRow dr in dt.Rows)
            {
                Shippers ship = new Shippers();
                ship.ShipperID = int.Parse(dr["ShipperID"].ToString());
                ship.CompanyName = dr["CompanyName"].ToString();
                listship.Add(ship);
            }
            ViewBag.Shippers = listship;


            Sql = "SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, Customers.CompanyName AS CustomerName, Employees.LastName + ' ' + Employees.FirstName AS EmployeeName, Shippers.CompanyName FROM     Orders LEFT OUTER JOIN Shippers ON Orders.ShipVia = Shippers.ShipperID LEFT OUTER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID LEFT OUTER JOIN Customers ON Orders.CustomerID = Customers.CustomerID WHERE OrderID=" + id;
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            Orders order = new Orders();
            if (dt.Rows.Count > 0)
            {
                order.OrderID = int.Parse(dt.Rows[0]["OrderID"].ToString());
                order.CustomerID = dt.Rows[0]["CustomerID"].ToString();
                order.CustomerName = dt.Rows[0]["CustomerName"].ToString();
                order.EmployeeID = int.Parse(dt.Rows[0]["EmployeeID"].ToString());
                order.EmployeeName = dt.Rows[0]["EmployeeName"].ToString();
                order.OrderDate = (DateTime)dt.Rows[0]["OrderDate"];
                order.RequiredDate = (DateTime)dt.Rows[0]["RequiredDate"];

                if (dt.Rows[0]["ShippedDate"].ToString() != "" && dt.Rows[0]["ShippedDate"] != null)
                {
                    order.ShippedDate = Convert.ToDateTime(dt.Rows[0]["ShippedDate"]);
                }

                order.ShipVia = int.Parse(dt.Rows[0]["ShipVia"].ToString());
                order.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                order.Freight = decimal.Parse(dt.Rows[0]["Freight"].ToString());
                order.ShipName = dt.Rows[0]["ShipName"].ToString();
                order.ShipAddress = dt.Rows[0]["ShipAddress"].ToString();
                order.ShipCity = dt.Rows[0]["ShipCity"].ToString();
                order.ShipRegion = dt.Rows[0]["ShipRegion"].ToString();
                order.ShipPostalCode = dt.Rows[0]["ShipPostalCode"].ToString();
                order.ShipCountry = dt.Rows[0]["ShipCountry"].ToString();
            }

            List<OrderDetails> listorders = new List<OrderDetails>();


            Sql = "SELECT [Order Details].OrderID, [Order Details].ProductID, [Order Details].UnitPrice, [Order Details].Quantity, [Order Details].Discount, Products.ProductName FROM     [Order Details] INNER JOIN Products ON [Order Details].ProductID = Products.ProductID WHERE OrderID=" + id;
            da = new SqlDataAdapter(Sql, conn);
            dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                OrderDetails details = new OrderDetails();
                details.OrderID = int.Parse(item["OrderID"].ToString());
                details.ProductID = int.Parse(item["ProductID"].ToString());
                details.ProductName = item["ProductName"].ToString();
                details.UnitPrice = float.Parse(item["UnitPrice"].ToString());
                details.Quantity = int.Parse(item["Quantity"].ToString());
                details.Discount = Decimal.Parse(item["Discount"].ToString());
                listorders.Add(details);
            }



            Session["listorders"] = listorders;

            try
            {
                Sql = "UPDATE  Orders SET CustomerID = N'" + obj.CustomerID + "' ,EmployeeID=" + obj.EmployeeID + ",OrderDate='" + obj.OrderDate.ToString("MM/dd/yyyy") + "',RequiredDate='" + obj.RequiredDate.ToString("MM/dd/yyyy") + "',ShippedDate='" + obj.ShippedDate.ToString("MM/dd/yyyy") + "',ShipVia=" + obj.ShipVia + ",Freight=" + obj.Freight + ",ShipName=N'" + obj.ShipName + "',ShipAddress=N'" + obj.ShipAddress + "',ShipCity=N'" + obj.ShipCity + "',ShipRegion=N'" + obj.ShipRegion + "',ShipPostalCode=N'" + obj.ShipPostalCode + "',ShipCountry=N'" + obj.ShipCountry + "' WHERE  OrderID="+id;
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = Sql;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                //Xoa dữ lieu cũ của Order
                Sql = "DELETE FROM Order Details WHERE OrderID=" + id;
                cmd.CommandText = Sql;
                cmd.ExecuteNonQuery();

                //Them lại dữ liệu vào 
                List<OrderDetails> orderDetails = new List<OrderDetails>();
                int i = 0;
                if (Session["listorders"] != null)
                {
                    orderDetails = (List<OrderDetails>)Session["listorders"];
                }
                foreach (var item in orderDetails)
                {
                    Sql = "INSERT INTO  [Order Details] (OrderID,ProductID,UnitPrice,Quantity,Discount) VALUES (" + id + "," + item.ProductID + ", " + item.UnitPrice + "," + item.Quantity + ", " + item.Discount + " ";
                    cmd.CommandText = Sql;
                    cmd.ExecuteNonQuery();
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Orders/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
