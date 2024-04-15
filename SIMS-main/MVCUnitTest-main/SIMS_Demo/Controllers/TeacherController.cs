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
        public IActionResult Edit(User model)
        {
            // Đọc dữ liệu từ file data.json
            string jsonData = System.IO.File.ReadAllText("data.json");
            var users = JsonSerializer.Deserialize<User[]>(jsonData);

            // Tìm user trong mảng users dựa trên Id
            var existingUser = Array.Find(users, u => u.Id == model.Id);

            if (existingUser != null)
            {
                // Chỉ cập nhật trường Core và Status
                existingUser.Core = model.Core;
                existingUser.Status = model.Status;

                // Lưu dữ liệu đã thay đổi vào file data.json
                string updatedJsonData = JsonSerializer.Serialize(users);
                System.IO.File.WriteAllText("data.json", updatedJsonData);

                // Chuyển hướng về trang thành công hoặc trang khác tùy theo nhu cầu của bạn
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý khi không tìm thấy người dùng
                return RedirectToAction("Error");
            }
        }




        public IActionResult Index()
        {
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
