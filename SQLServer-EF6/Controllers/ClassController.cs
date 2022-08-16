using Microsoft.AspNetCore.Mvc;
using SQLServer_EF6.Models;
using SQLServer_EF6.Services;

namespace SQLServer_EF6.Controllers
{
    public class ClassController : Controller
    {
        private readonly GeneralDBContext _db;
        public ClassController(GeneralDBContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            List<ClassModel> listClass = _db.getAllClass();
            return View(listClass);
        }

        

        public IActionResult Edit(int id)
        {
            ClassModel c = _db.getClassById(id);
            if (c == null) return NotFound();
            return View(c);
        }
    }
}
