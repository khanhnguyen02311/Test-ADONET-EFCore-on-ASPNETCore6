using Microsoft.AspNetCore.Mvc;
using SQLServer_ADONET.Models;
using SQLServer_ADONET.Services;

namespace SQLServer_ADONET.Controllers
{
    public class StudentController : Controller
    {
        private readonly IApplicationService applicationService;
        private List<ClassModel> listClass = new List<ClassModel>();
        private List<StudentModel> listStudent = new List<StudentModel>();

        public StudentController(IApplicationService s) { this.applicationService = s; }

        [HttpGet]
        public IActionResult Index(string sortOrder, string searchString)
        {
            listStudent = applicationService.getAllStudent();
            //searching
            ViewData["CurrentFilter"] = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                listStudent = listStudent.Where(s => s.Lastname.ToLower().Contains(searchString.ToLower()) || s.Firstname.ToLower().Contains(searchString.ToLower())).ToList();
            }

            //sorting
            ViewData["IDSort"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["LastnameSort"] = sortOrder == "lastname_asc" ? "lastname_desc" : "lastname_asc";
            ViewData["ClassSort"] = sortOrder == "class_asc" ? "class_desc" : "class_asc";
            switch (sortOrder)
            {
                case "id_desc":
                    listStudent = listStudent.OrderByDescending(s => s.ID).ToList();
                    break;
                case "lastname_desc":
                    listStudent = listStudent.OrderByDescending(s => s.Lastname).ToList();
                    break;
                case "lastname_asc":
                    listStudent = listStudent.OrderBy(s => s.Lastname).ToList();
                    break;
                case "class_desc":
                    listStudent = listStudent.OrderByDescending(s => s.Classname).ToList();
                    break;
                case "class_asc":
                    listStudent = listStudent.OrderBy(s => s.Classname).ToList();
                    break;
                default:
                    listStudent = listStudent.OrderBy(s => s.ID).ToList();
                    break;
            }
            return View(listStudent);
        }


        public IActionResult Create()
        {
            listClass = applicationService.getAllClass();
            ViewData["Classes"] = listClass;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                if (student.ClassID == -1) student.ClassID = null;
                if (applicationService.addStudent(student))
                    return RedirectToAction("Index");
                return View(student);
            }
            listClass = applicationService.getAllClass();
            ViewData["Classes"] = listClass;
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            StudentModel? student = applicationService.getStudentByID((int)id);
            if (student == null) return NotFound();
            listClass = applicationService.getAllClass();
            ViewData["Classes"] = listClass;
            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentModel student)
        {
            if (ModelState.IsValid)
            {
                if (student.ClassID == -1) student.ClassID = null;
                if (applicationService.updateStudent(student))
                    return RedirectToAction("Index");
                return View(student);
            }
            listClass = applicationService.getAllClass();
            ViewData["Classes"] = listClass;
            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            if (!applicationService.deleteStudent((int)id)) return NotFound();
            return RedirectToAction("Index");
        }
    }
}
