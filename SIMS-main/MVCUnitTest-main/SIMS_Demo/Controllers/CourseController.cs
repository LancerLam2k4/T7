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
        /*[HttpGet]
        public IActionResult Edit(int id)
        {
            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            var existingCourse = courses.FirstOrDefault(c => c.Id == course.Id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.Name = course.Name;
            existingCourse.Teacher = course.Teacher;
            existingCourse.RoomNumber = course.RoomNumber;
            existingCourse.Time = course.Time;

            //UpdateCourseJsonFile();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            courses.Remove(course);

            //UpdateCourseJsonFile();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult NewCourse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewCourse(Course course)
        {
            courses.Add(course);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(courses, options);
            using (StreamWriter writer = new StreamWriter("data.json"))
            {
                writer.Write(jsonString);
            }
            return Content(jsonString);
        }*/
    }
}
