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
        static List<User>? std = new List<User>();
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userToEdit = std.FirstOrDefault(u => u.Id == id);

            if (userToEdit == null)
            {
                // Nếu không tìm thấy người dùng, trả về một trang lỗi hoặc trang không tìm thấy
                return NotFound();
            }

            // Trả về view chỉnh sửa và truyền vào người dùng cần chỉnh sửa
            return View(userToEdit);
        }
        [HttpPost]
        public IActionResult Edit(User editedUser)
        {
            // Khởi tạo danh sách std nếu chưa có
            if (std == null)
            {
                std = ReadFileToTeacherList("data.json");
            }

            if (ModelState.IsValid)
            {
                // Tìm và cập nhật thông tin người dùng trong danh sách std
                var userToEdit = std.FirstOrDefault(u => u.Id == editedUser.Id);

                if (userToEdit != null)
                {
                    userToEdit.Core = editedUser.Core;
                    userToEdit.Status = editedUser.Status;

                    // Ghi danh sách std đã cập nhật vào tệp data.json
                    WriteUserListToFile(std, "data.json");

                    // Chuyển hướng đến trang Index sau khi cập nhật thành công
                    return RedirectToAction("Index");
                }
                else
                {
                    // Trả về lỗi nếu không tìm thấy người dùng
                    return NotFound();
                }
            }
            else
            {
                // Trả về view chỉnh sửa với thông báo lỗi nếu ModelState không hợp lệ
                return View(editedUser);
            }
        }




        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("Role");
            std = ReadFileToTeacherList("data.json");
            return View(std);
        }

        public static List<User>? ReadFileToTeacherList(String filePath)
        {
            // Read a file
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(readText);
        }

        //click Hyperlink
        [HttpGet]
        public IActionResult NewTeacher()
        {
            return View();
        }
        public void WriteUserListToFile(List<User> users, string filePath)
        {
            // Chuyển danh sách người dùng thành chuỗi JSON
            string jsonString = JsonSerializer.Serialize(users);

            // Ghi chuỗi JSON vào tệp
            System.IO.File.WriteAllText(filePath, jsonString);
        }


    }
}
