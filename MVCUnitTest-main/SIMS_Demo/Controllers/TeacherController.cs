using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SIMS_Demo.Models;


namespace SIMS_Demo.Controllers
{
    public class TeacherController : Controller
    {
        static List<Teacher>? teachers = new List<Teacher>();

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            //read Teachers from a file
            teachers = ReadFileToTeacherList("Teacher.json");
            //search and delete
            var result = teachers.FirstOrDefault(t => t.Id == Id);
            if (result != null)
            {
                teachers.Remove(result);
                //update json file
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(teachers, options);
                using (StreamWriter writer = new StreamWriter("Teacher.json"))
                {
                    writer.Write(jsonString);
                }
            }
            return RedirectToAction("Index");


        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("Role");
            teachers = ReadFileToTeacherList("Teacher.json");
            return View(teachers);
        }

        public static List<Teacher>? ReadFileToTeacherList(String filePath)
        {
            // Read a file
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Teacher>>(readText);
        }

        //click Hyperlink
        [HttpGet]
        public IActionResult NewTeacher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewTeacher(Teacher teacher)
        {
            teachers.Add(teacher);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(teachers, options);
            using (StreamWriter writer = new StreamWriter("Teacher.json"))
            {
                writer.Write(jsonString);
            }
            return Content(jsonString);
        }

    }
}
