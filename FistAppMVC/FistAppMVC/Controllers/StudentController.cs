using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FistAppMVC.Models;

namespace FistAppMVC.Controllers
{
    public class StudentController : Controller
    {
        static int ID = 0;
        static List<Student> students = new List<Student>();
        // GET: Student
        public ActionResult Index()
        {
            return View(students);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student st)
        {
            ID++;
            st.StudentID = ID;
            students.Add(st);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Student st = new Student();
            foreach (Student student in students)
            {
                if (student.StudentID == id)
                {
                    st = student;
                }
            }
            return View(st);
        }

        [HttpPost]
        public ActionResult Edit(Student st)
        {
            int index = students.FindIndex(n => n.StudentID == st.StudentID);
            if (index != -1)
            {
                students[index] = st; // Cập nhật giá trị
            }


            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            Student st = new Student();
            foreach (Student student in students)
            {
                if (student.StudentID == id)
                {
                    st = student;
                }
            }
            return View(st);
        }

        public ActionResult Delete(int id)
        {
            Student st = new Student();
            foreach (Student student in students)
            {
                if (student.StudentID == id)
                {
                    st = student;
                }
            }
            return View(st);
        }

        [HttpPost]

        public ActionResult Delete(Student st)
        {
            int index = students.FindIndex(n => n.StudentID == st.StudentID);
            Student student = students[index];
            students.Remove(student); // Cập nhật giá trị



            return RedirectToAction("Index");
        }

    }




}