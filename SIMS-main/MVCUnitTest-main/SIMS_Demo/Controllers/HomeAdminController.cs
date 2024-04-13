using Microsoft.AspNetCore.Mvc;
using SIMS_Demo.Models;
using System.Text.Json;

namespace SIMS_Demo.Controllers
{
    public class HomeAdminController : Controller
    {
        static List<User>? std = new List<User>();
        public IActionResult Index(String userName, String userRole)
        {
            ViewBag.UserName = userName;
            ViewBag.Role = userRole;
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

                    // Kiểm tra giá trị Core từ form, nếu null thì sử dụng giá trị hiện tại của userToEdit
                    userToEdit.Core = editedUser.Core ?? userToEdit.Core;

                    // Kiểm tra giá trị Status từ form, nếu null thì sử dụng giá trị hiện tại của userToEdit
                    userToEdit.Status = editedUser.Status ?? userToEdit.Status;

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
        [HttpGet]
        public IActionResult NewStudent()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewStudent(User newUser)
        {
            try
            {
                // Đọc danh sách người dùng từ tệp JSON
                List<User> users = ReadFileToTeacherList("users.json");

                // Tạo ID cho người dùng mới (ví dụ: tìm ID lớn nhất trong danh sách và tăng lên 1)
                int newUserId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

                // Gán ID và vai trò mặc định cho người dùng mới
                newUser.Id = newUserId;

                // Thêm người dùng mới vào danh sách
                users.Add(newUser);

                // Cập nhật danh sách người dùng vào tệp JSON
                UpdateFileFromList("users.json", users);
                UpdateFileFromListData("data.json", users);

                // Thông báo đăng ký thành công
                ViewBag.SuccessMessage = "Đăng ký thành công!";

                // Chuyển hướng sau khi đăng ký thành công (ví dụ: về trang đăng nhập)
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Thông báo đăng ký thất bại
                ViewBag.ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại sau.";

                // Chuyển hướng về trang đăng ký
                return RedirectToAction("NewStudent");
            }
        }
        public static void UpdateFileFromList(string filePath, List<User> users)
        {
            string jsonData = JsonSerializer.Serialize(users);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
        public static void UpdateFileFromListData(string filePath, List<User> users)
        {
            // Tạo ra một danh sách mới chỉ chứa các thuộc tính cần thiết của User (loại bỏ Password và Role)
            var userSubset = users.Select(u => new
            {
                u.Id,
                u.Name,
                u.DoB,
                u.Email,
                u.Core,
                u.Status,
                u.Class,
                u.Major,
                u.Role
            }).ToList();

            string jsonData = JsonSerializer.Serialize(userSubset);
            System.IO.File.WriteAllText(filePath, jsonData);
        }























        //this is ListTecher
        [HttpGet]
        public IActionResult ListTeacher(String userName, String userRole)
        {
            ViewBag.UserName = userName;
            ViewBag.Role = userRole;
            std = ReadFileToTeacherList("data.json");
            return View(std);
        }
        public IActionResult EditTeacher(int id)
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
        public IActionResult EditTeacher(User editedUser)
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
                    return RedirectToAction("ListTeacher");
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
        [HttpGet]
        public IActionResult NewTeacher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewTeacher(User newUser)
        {
            try
            {
                // Đọc danh sách người dùng từ tệp JSON
                List<User> users = ReadFileToTeacherList("users.json");

                // Tạo ID cho người dùng mới (ví dụ: tìm ID lớn nhất trong danh sách và tăng lên 1)
                int newUserId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;

                // Gán ID và vai trò mặc định cho người dùng mới
                newUser.Id = newUserId;

                // Thêm người dùng mới vào danh sách
                users.Add(newUser);

                // Cập nhật danh sách người dùng vào tệp JSON
                UpdateFileFromList("users.json", users);
                UpdateFileFromListData("data.json", users);

                // Thông báo đăng ký thành công
                ViewBag.SuccessMessage = "Đăng ký thành công!";

                // Chuyển hướng sau khi đăng ký thành công (ví dụ: về trang đăng nhập)
                return RedirectToAction("ListTeacher");
            }
            catch (Exception ex)
            {
                // Thông báo đăng ký thất bại
                ViewBag.ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại sau.";

                // Chuyển hướng về trang đăng ký
                return RedirectToAction("NewTeacher");
            }
        }











        //This is ListCourse
        static List<Course>? course = new List<Course>();
        [HttpGet]
        public IActionResult ListCourse()
        {
            course = ReadFileToCourseList("course.json");
            return View(course);
        }
        public static List<Course>? ReadFileToCourseList(String filePath)
        {
            // Read a file
            string readText = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Course>>(readText);
        }
        [HttpGet]
        public IActionResult EditCourse(int id)
        {
            var courses = ReadFileToCourseList("course.json");
            var courseToEdit = courses.FirstOrDefault(c => c.Id == id);

            if (courseToEdit == null)
            {
                // Nếu không tìm thấy khóa học, trả về một trang lỗi hoặc trang không tìm thấy
                return NotFound();
            }

            // Trả về view chỉnh sửa và truyền vào khóa học cần chỉnh sửa
            return View(courseToEdit);
        }
        [HttpPost]
        public IActionResult EditCourse(Course editedcourse)
        {
            if (ModelState.IsValid)
            {
                var courses = ReadFileToCourseList("course.json");

                var courseToEdit = courses.FirstOrDefault(c => c.Id == editedcourse.Id);

                if (courseToEdit != null)
                {
                    // Cập nhật thông tin của khóa học
                    courseToEdit.Subject = editedcourse.Subject;
                    courseToEdit.Time = editedcourse.Time;
                    courseToEdit.Slot = editedcourse.Slot;
                    courseToEdit.RoomNumber = editedcourse.RoomNumber;
                    courseToEdit.Teacher = editedcourse.Teacher;

                    // Ghi lại danh sách khóa học đã cập nhật vào tệp data.json
                    WriteCourseListToFile(courses, "course.json");

                    // Chuyển hướng đến trang ListTeacher sau khi cập nhật thành công
                    return RedirectToAction("ListCourse");
                }
                else
                {
                    // Trả về lỗi nếu không tìm thấy khóa học
                    return NotFound();
                }
            }
            else
            {
                // Trả về view chỉnh sửa với thông báo lỗi nếu ModelState không hợp lệ
                return View(editedcourse);
            }
        }

        public void WriteCourseListToFile(List<Course> courses, string filePath)
        {
            // Chuyển danh sách khóa học thành chuỗi JSON
            string jsonString = JsonSerializer.Serialize(courses);

            // Ghi chuỗi JSON vào tệp
            System.IO.File.WriteAllText(filePath, jsonString);
        }
        [HttpGet]
        public IActionResult DeleteCourse(int id)
        {
            // Đọc danh sách khóa học từ tệp course.json
            List<Course> courses = ReadFileToCourseList("course.json");
            // Tìm khóa học cần xóa
            Course courseToRemove = courses.FirstOrDefault(c => c.Id == id);
            if (courseToRemove != null)
            {
                // Xóa khóa học khỏi danh sách
                courses.Remove(courseToRemove);
                // Ghi danh sách khóa học đã cập nhật vào tệp course.json
                WriteCourseListToFile(courses, "course.json");
            }

            return RedirectToAction("ListCourse");
        }
        [HttpGet]
        public IActionResult NewCourse()
        {
            std = ReadFileToTeacherList("data.json");
            var teachers = std.Where(u => u.Role == "Teacher").Select(u => u.Name).ToList();
            ViewBag.Teachers = teachers;
            return View();
        }
        [HttpPost]
        public IActionResult NewCourse(Course newCourse)
        {
            if (ModelState.IsValid)
            {
                // Đọc danh sách khóa học từ tệp course.json
                List<Course> courses = ReadFileToCourseList("course.json");

                // Tạo ID cho khóa học mới (ví dụ: tìm ID lớn nhất trong danh sách và tăng lên 1)
                int newCourseId = courses.Count > 0 ? courses.Max(c => c.Id) + 1 : 1;

                // Gán ID cho khóa học mới
                newCourse.Id = newCourseId;

                // Thêm khóa học mới vào danh sách
                courses.Add(newCourse);

                // Ghi danh sách khóa học đã cập nhật vào tệp course.json
                WriteCourseListToFile(courses, "course.json");

                // Chuyển hướng sau khi thêm khóa học thành công (ví dụ: về trang danh sách khóa học)
                return RedirectToAction("ListCourse");
            }
            else
            {
                // Trả về view NewCourse với model chưa hợp lệ nếu ModelState không hợp lệ
                return View(newCourse);
            }
        }



    }
}
