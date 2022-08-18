using Microsoft.AspNetCore.Mvc;
using SQLServer_ADONET.Models;
using SQLServer_ADONET.Services;
using System.Dynamic;

namespace SQLServer_ADONET.Controllers
{
    public class ClassController : Controller
    {
        private readonly IApplicationService applicationService;
        private List<ClassModel> listClass = new List<ClassModel>();

        public ClassController(IApplicationService applicationService) { this.applicationService = applicationService; }

        [HttpGet]
        public IActionResult Index(string sortOrder, string searchClass)
        {
            listClass = applicationService.getAllClass();
            ViewData["searchFilter"] = searchClass;
            if (!String.IsNullOrEmpty(searchClass))
                listClass = listClass
                    .Where(e => e.Classname.ToLower().Contains(searchClass.ToLower()))
                    .ToList();
            ViewData["IDSort"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSort"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
            ViewData["AmountSort"] = sortOrder == "amount_desc" ? "amount_asc" : "amount_desc";
            switch (sortOrder)
            {
                case "id_desc": listClass = listClass.OrderByDescending(e => e.ID).ToList(); break;
                case "name_desc": listClass = listClass.OrderByDescending(e => e.Classname).ToList(); break;
                case "name_asc": listClass = listClass.OrderBy(e => e.Classname).ToList(); break;
                case "amount_desc": listClass = listClass.OrderByDescending(e => e.NumStudent).ToList(); break;
                case "amount_asc": listClass = listClass.OrderBy(e => e.NumStudent).ToList(); break;
                default: listClass = listClass.OrderBy(e => e.ID).ToList(); break;
            }
            return View(listClass);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string classname)
        {
            if (!applicationService.addClass(classname)) return RedirectToAction("Create");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            if (!applicationService.deleteClass((int)id)) return NotFound();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id, string newName)
        {
            if (id == null) return NotFound();
            dynamic model = new ExpandoObject(); //multiple models in 1 mvc page
            if (newName != null) applicationService.updateClass((int)id, newName);
            model.Class = applicationService.getClassByID((int)id);
            model.Student = applicationService.getStudentByClass((int)id);
            model.UnknownStudent = applicationService.getStudentByClass(-1);
            if (model.Class == null) return NotFound();
            return View(model);
        }

        public IActionResult RemoveOrAddClass(int? id, int classid, bool isUnknown = false)
        {
            if (id == null) return NotFound();
            if (!applicationService.changeStudentClass((int)id, isUnknown ? classid : -1)) return NotFound();
            return RedirectToAction("Edit", new { id = classid });
        }
    }
}
