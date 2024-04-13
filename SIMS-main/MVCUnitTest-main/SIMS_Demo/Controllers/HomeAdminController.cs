using Microsoft.AspNetCore.Mvc;

namespace SIMS_Demo.Controllers
{
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
