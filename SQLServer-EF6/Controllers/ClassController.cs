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

        [HttpGet]
        public IActionResult Index(string sortOrder, string searchClass)
        {
            List<ClassModel> listClass = _db.getAllClass();
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
                case "id_desc": listClass = listClass.OrderByDescending(e => e.Id).ToList(); break;
                case "name_desc": listClass = listClass.OrderByDescending(e => e.Classname).ToList(); break;
                case "name_asc": listClass = listClass.OrderBy(e => e.Classname).ToList(); break;
                case "amount_desc": listClass = listClass.OrderByDescending(e => e.NumStudent).ToList(); break;
                case "amount_asc": listClass = listClass.OrderBy(e => e.NumStudent).ToList(); break;
                default: listClass = listClass.OrderBy(e => e.Id).ToList(); break;
            }
            return View(listClass);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClassModel c)
        {
            if (c.Classname != null && 
                c.Classname != "" && 
                _db.Classes.SingleOrDefault(e => e.Classname == c.Classname) == null)
            {
                _db.Classes.Add(c);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c);
        }

        public IActionResult Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var students = _db.Students.Where(e => e.ClassId == id);
                    foreach (var student in students) student.ClassId = null;
                    _db.SaveChanges();

                    var c = _db.Classes.Find(id);
                    _db.Classes.Remove(c);
                    _db.SaveChanges();
                    transaction.Commit();
                    
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public IActionResult Edit(int id, string newName)
        {
            ClassModel c = _db.getClassById(id);
            if (c == null || c.Id == 0) return NotFound();
            if (newName != null && 
                newName != "" && 
                _db.Classes.FirstOrDefault(e => e.Classname == newName) == null)
            {
                c.Classname = newName;
                _db.SaveChanges();
            }
            ViewData["NoClassStudent"] = _db.getStudentsByClass(null);
            return View(c);
        }

        public IActionResult RemoveOrAddClass(int id, int classid, bool isUnknown = false)
        {
            if (isUnknown) _db.updateStudentClass(id, classid);
            else _db.updateStudentClass(id, null);
            return RedirectToAction("Edit", new {id = classid});
        }
    }
}
