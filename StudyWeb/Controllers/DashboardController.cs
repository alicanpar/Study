using Microsoft.AspNetCore.Mvc;

namespace StudyWeb.Controllers
{
    
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult xxx()
        {
            return View();
        }
        public PartialViewResult _Partial1()
        {
            return PartialView();
        }
    }
}
