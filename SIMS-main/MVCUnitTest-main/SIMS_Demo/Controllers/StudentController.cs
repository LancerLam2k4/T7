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
    public class StudentController : Controller
    {
        private static int nextId = 1; // Biến để lưu trữ giá trị Id tiếp theo

        private List<Student> ReadFileToStudentList(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return new List<Student>();
            }

            string jsonText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Student>>(jsonText) ?? new List<Student>();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            List<Student> students = ReadFileToStudentList("data.json");
            Student studentToRemove = students.FirstOrDefault(s => s.Id == id);
            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                string jsonString = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText("data.json", jsonString);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("Email");
            ViewBag.UserRole = HttpContext.Session.GetString("Role");
            List<Student> students = ReadFileToStudentList("data.json");
            return View(students);
        }

        [HttpGet]
        public IActionResult NewStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewStudent(Student student)
        {
            student.Id = nextId;
            nextId++;

            List<Student> students = ReadFileToStudentList("data.json");
            students.Add(student);
            string jsonString = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText("data.json", jsonString);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            List<Student> students = ReadFileToStudentList("data.json");
            Student student = students.FirstOrDefault(s => s.Id == id);
            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            List<Student> students = ReadFileToStudentList("data.json");
            Student student = students.FirstOrDefault(s => s.Id == id);
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student updatedStudent)
        {
            try
            {
                List<Student> students = ReadFileToStudentList("data.json");
                Student existingStudent = students.FirstOrDefault(s => s.Id == updatedStudent.Id);
                if (existingStudent != null)
                {
                    existingStudent.Name = updatedStudent.Name;
                    existingStudent.DoB = updatedStudent.DoB;
                    existingStudent.Email = updatedStudent.Email;
                    existingStudent.StudentID = updatedStudent.StudentID;
                    existingStudent.Class = updatedStudent.Class;
                    existingStudent.Major = updatedStudent.Major;
                    // Cập nhật các thuộc tính khác nếu cần

                    string jsonString = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
                    System.IO.File.WriteAllText("data.json", jsonString);

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý lỗi tại đây
                return StatusCode(500); // Trả về mã lỗi 500 nếu có lỗi xảy ra
            }
        }
    }
}
