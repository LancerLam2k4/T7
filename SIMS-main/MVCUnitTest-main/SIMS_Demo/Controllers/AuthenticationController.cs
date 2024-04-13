using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;
using System.Text.Json;

namespace SIMS_Demo.Controllers
{

    public class AuthenticationController : Controller
    {
        static List<User>? user = new List<User>();
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            List<User> users = ReadFileToList("users.json");
            var result = users.FirstOrDefault(u => u.Email == user.Email && user.Password == user.Password);
            if (result != null)
            {
                HttpContext.Session.SetString("Email", result.Email);
                HttpContext.Session.SetString("Role", result.Role);
                HttpContext.Session.SetString("Name",result.Name);
                if (result.Role == "Admin")
                {
                    return RedirectToAction("Index", "HomeAdmin", new { userName = result.Name });
                }
                else if (result.Role == "Student")
                {
                    // Chuyển hướng đến trang phù hợp cho vai trò "Student" (nếu cần)
                    return RedirectToAction("Index", "HomeStudent",new { userName = result.Name });
                }
                else if (result.Role == "Teacher")
                {
                    // Chuyển hướng đến trang phù hợp cho vai trò "Student" (nếu cần)
                    return RedirectToAction("Index", "Teacher", new { userName = result.Name });
                }
                else
                {
                    // Trường hợp Role không phù hợp, bạn có thể xử lý tại đây
                    ViewBag.error = "Invalid user role";
                    return View();
                }
            }
            else
            {
                ViewBag.error = "Invalid user";
                return View();
            }
        }
        public static List<User>? ReadFileToList(String filePath)
        {
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(readText);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User newUser)
        {
            try
            {
                // Đọc danh sách người dùng từ tệp JSON
                List<User> users = ReadFileToList("users.json");

                // Tạo ID cho người dùng mới (ví dụ: tìm ID lớn nhất trong danh sách và tăng lên 1)
                int newUserId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

                // Gán ID và vai trò mặc định cho người dùng mới
                newUser.Id = newUserId;
                newUser.Role = "Student";

                // Thêm người dùng mới vào danh sách
                users.Add(newUser);

                // Cập nhật danh sách người dùng vào tệp JSON
                UpdateFileFromList("users.json", users);
                UpdateFileFromListData("data.json", users);

                // Thông báo đăng ký thành công
                ViewBag.SuccessMessage = "Đăng ký thành công!";

                // Chuyển hướng sau khi đăng ký thành công (ví dụ: về trang đăng nhập)
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                // Thông báo đăng ký thất bại
                ViewBag.ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại sau.";

                // Chuyển hướng về trang đăng ký
                return RedirectToAction("Register");
            }
        }

        public static void UpdateFileFromList(string filePath, List<User> users)
        {
            string jsonData = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
        public static void UpdateFileFromListData(string filePath, List<User> users)
{
    string jsonData = JsonSerializer.Serialize(users);
    System.IO.File.WriteAllText(filePath, jsonData);
}


    }
}
