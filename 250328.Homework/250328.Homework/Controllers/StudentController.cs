using System.Collections.Generic;
using System.Web.Mvc;

namespace StudentManagementMVC.Controllers
{
    public class StudentController : Controller
    {
        // Danh sách sinh viên được lưu trực tiếp trong Controller
        private static List<Dictionary<string, string>> students = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string> { { "Id", "1" }, { "Name", "Nguyen Van A" }, { "Age", "20" }, { "Email", "a@gmail.com" } },
            new Dictionary<string, string> { { "Id", "2" }, { "Name", "Le Thi B" }, { "Age", "21" }, { "Email", "b@gmail.com" } },
            new Dictionary<string, string> { { "Id", "3" }, { "Name", "Tran Van C" }, { "Age", "19" }, { "Email", "c@gmail.com" } },
            new Dictionary<string, string> { { "Id", "4" }, { "Name", "Pham Van D" }, { "Age", "22" }, { "Email", "d@gmail.com" } },
            new Dictionary<string, string> { { "Id", "5" }, { "Name", "Hoang Thi E" }, { "Age", "20" }, { "Email", "e@gmail.com" } }
        };

        public ActionResult Index()
        {
            string html = "<h2>Danh sách sinh viên</h2>";
            html += "<table border='1' width='70%'><tr><th>ID</th><th>Họ và tên</th><th>Tuổi</th><th>Email</th></tr>";

            foreach (var student in students)
            {
                html += $"<tr><td>{student["Id"]}</td><td>{student["Name"]}</td><td>{student["Age"]}</td><td>{student["Email"]}</td></tr>";
            }

            html += "</table><br /><a href='/Student/Create'>Thêm sinh viên</a>";
            return Content(html, "text/html");
        }

        public ActionResult Create()
        {
            string html = @"
                <h2>Nhập thông tin sinh viên</h2>
                <form action='/Student/CreateStudent' method='post'>
                    Họ và tên: <input type='text' name='name' required /><br /><br />
                    Tuổi: <input type='number' name='age' required /><br /><br />
                    Email: <input type='email' name='email' required /><br /><br />
                    <input type='submit' value='Thêm sinh viên' />
                </form>
            ";
            return Content(html, "text/html");
        }

        [HttpPost]
        public ActionResult CreateStudent()
        {
            string name = Request.Form["name"];
            string age = Request.Form["age"];
            string email = Request.Form["email"];
            string id = (students.Count + 1).ToString();

            students.Add(new Dictionary<string, string>
            {
                { "Id", id },
                { "Name", name },
                { "Age", age },
                { "Email", email }
            });

            return RedirectToAction("Index");
        }
    }
}
