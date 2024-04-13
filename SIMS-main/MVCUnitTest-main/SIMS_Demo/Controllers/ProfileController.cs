using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;
using System.Text.Json;

namespace SIMS_Demo.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize] // Đảm bảo chỉ người dùng đã xác thực mới có quyền truy cập
        public IActionResult Profile()
        {
            // Lấy thông tin người dùng từ phiên làm việc
            var email = HttpContext.Session.GetString("Email");

            // Đọc danh sách người dùng từ tệp JSON
            var users = ReadFileToList("users.json");

            // Tìm người dùng theo email
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                // Chuyển thông tin người dùng vào view để hiển thị
                return View(user);
            }

            // Xử lý khi không tìm thấy thông tin người dùng
            return RedirectToAction("Error");
        }
        public static List<User>? ReadFileToList(String filePath)
        {
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(readText);
        }

    }
}
