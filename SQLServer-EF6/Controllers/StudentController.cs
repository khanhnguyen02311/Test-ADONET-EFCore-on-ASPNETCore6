using Microsoft.AspNetCore.Mvc;
using SQLServer_EF6.Models;
using SQLServer_EF6.Services;

namespace SQLServer_EF6.Controllers
{
    public class StudentController : Controller
    {
        private readonly GeneralDBContext _db;
        public StudentController(GeneralDBContext context)
        {
            _db = context;
        }
        [HttpGet]
        public IActionResult Index(string sortOrder, string searchString)
        {
            var listStudent = _db.getAllStudent();

            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                listStudent = listStudent.Where(s => s.Lastname.ToLower().Contains(searchString.ToLower()) || 
                                                     s.Firstname.ToLower().Contains(searchString.ToLower())).ToList();
            }
            
            //sorting
            ViewData["IDSort"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["LastnameSort"] = sortOrder == "lastname_asc" ? "lastname_desc" : "lastname_asc";
            ViewData["ClassSort"] = sortOrder == "class_asc" ? "class_desc" : "class_asc";
            switch (sortOrder)
            {
                case "id_desc":
                    listStudent = listStudent.OrderByDescending(s => s.Id).ToList();
                    break;
                case "lastname_desc":
                    listStudent = listStudent.OrderByDescending(s => s.Lastname).ToList();
                    break;
                case "lastname_asc":
                    listStudent = listStudent.OrderBy(s => s.Lastname).ToList();
                    break;
                case "class_desc":
                    listStudent = listStudent.OrderByDescending(s => s.Class != null ? s.Class.Classname : "").ToList();
                    break;
                case "class_asc":
                    listStudent = listStudent.OrderBy(s => s.Class != null ? s.Class.Classname : "").ToList();
                    break;
                default:
                    listStudent = listStudent.OrderBy(s => s.Id).ToList();
                    break;
            }
            return View(listStudent);
        }

        
        public IActionResult Create()
        {
            ViewData["Classes"] = _db.getAllClass();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                _db.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            
            var student = _db.getStudentById((int)id);
            if (student == null) return NotFound();
            
            ViewData["Classes"] = _db.getAllClass();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                _db.Update(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public IActionResult Delete(int id)
        {
            var student = _db.getStudentById(id);
            if (student == null) return NotFound();
            _db.Remove(student);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
