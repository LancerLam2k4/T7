using Microsoft.AspNetCore.Mvc;

namespace SIMS_Demo.Controllers
{
    public class HomeStudentController : Controller
    {
        public IActionResult Index(string userName)
        {
            ViewBag.UserName = userName;
            return View();
        }
    }
}
