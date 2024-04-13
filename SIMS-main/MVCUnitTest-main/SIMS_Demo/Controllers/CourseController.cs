using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;

namespace SIMS_Demo.Controllers
{
    public class CourseController : Controller
    {
        static List<Course> courses = new List<Course>();

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("Role");
            courses = ReadFileToCourseList("course.json");
            return View(courses);
        }
        public static List<Course>? ReadFileToCourseList(String filePath)
        {
            // Read a file
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Course>>(readText);
        }
        public IActionResult TimeTable()
        {
            var course = ReadFileToCourseList("course.json").OrderBy(c => c.Time).ToList();
            return View(course);
        }
       
    }
}
