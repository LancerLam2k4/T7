using Microsoft.AspNetCore.Mvc;

namespace SIMS_Demo.Controllers
{
    public class HomeStudentController : Controller
    {
        public IActionResult Index(string userName,string userRole)
        {
            ViewBag.UserName = userName;
            ViewBag.Role = userRole;
            return View();
        }
    }
}
