using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;
using System.Text.Json;

namespace SIMS_Demo.Controllers
{
    public class HomeAdminController : Controller
    {
        static List<User>? std = new List<User>();
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
            if (ModelState.IsValid)
            {
                var users = ReadFileToTeacherList("data.json");

                var userToEdit = users.FirstOrDefault(u => u.Id == editedUser.Id);

                if (userToEdit != null)
                {
                    // Cập nhật thông tin của người dùng
                    userToEdit.Name = editedUser.Name;
                    userToEdit.DoB = editedUser.DoB;
                    userToEdit.Email = editedUser.Email;
                    userToEdit.Core = editedUser.Core;
                    userToEdit.Status = editedUser.Status;
                    userToEdit.Class = editedUser.Class;
                    userToEdit.Major = editedUser.Major;
                    userToEdit.Password = editedUser.Password;
                    userToEdit.Role = editedUser.Role;

                    // Ghi lại danh sách người dùng đã cập nhật vào tệp data.json
                    WriteUserListToFile(users, "data.json");

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
        public void WriteUserListToFile(List<User> users, string filePath)
        {
            // Chuyển danh sách người dùng thành chuỗi JSON
            string jsonString = JsonSerializer.Serialize(users);

            // Ghi chuỗi JSON vào tệp
            System.IO.File.WriteAllText(filePath, jsonString);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Đọc danh sách học sinh từ tệp users.json
            List<User> users = ReadFileToTeacherList("users.json");
            // Tìm học sinh cần xóa
            User userToRemove = users.FirstOrDefault(u => u.Id == id);
            if (userToRemove != null)
            {
                // Xóa học sinh khỏi danh sách
                users.Remove(userToRemove);
                // Ghi danh sách học sinh đã cập nhật vào tệp users.json
                WriteUserListToFile(users, "users.json");
            }

            // Đọc danh sách học sinh từ tệp data.json
            List<User> students = ReadFileToTeacherList("data.json");
            // Tìm học sinh cần xóa
            User studentToRemove = students.FirstOrDefault(s => s.Id == id);
            if (studentToRemove != null)
            {
                // Xóa học sinh khỏi danh sách
                students.Remove(studentToRemove);
                // Ghi danh sách học sinh đã cập nhật vào tệp data.json
                WriteUserListToFile(students, "data.json");
            }

            return RedirectToAction("Index");
        }

    }
}
