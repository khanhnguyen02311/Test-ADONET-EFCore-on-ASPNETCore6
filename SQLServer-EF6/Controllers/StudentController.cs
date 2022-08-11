using Microsoft.AspNetCore.Mvc;

namespace SQLServer_EF6.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
