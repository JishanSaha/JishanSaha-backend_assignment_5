using Microsoft.AspNetCore.Mvc;
using cumulative3.Models;
using cumulative3.Controllers;
namespace cumulative3.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        //GET : TeacherPage/List/SearchKey={SearchKey}
        [HttpGet]
        public IActionResult List(string SearchKey)
        {
            List<Teacher> Teachers = _api.ListTeachers(SearchKey);
            return View(Teachers);
        }

        //GET : AuthorPage/Show/{id}
        [HttpGet]
        public IActionResult Show(int id)
        {
            Teacher SelectedAuthor = _api.FindTeacher(id);
            return View(SelectedAuthor);
        }
        // GET : TeacherPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);

            // redirects to "Show" action on "Teacher" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = TeacherId });
        }
        // GET : TeacherPage/Delete/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Teacher SelectedAuthor = _api.FindTeacher(id);
            return View(SelectedAuthor);
        }

        // POST: AuthorPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int AuthorId = _api.DeleteTeacher(id);
            // redirects to list action
            return RedirectToAction("List");
        }
        
        // GET : TeacherPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Teacher SelectedAuthor = _api.FindTeacher(id);
            return View(SelectedAuthor);
        }

        // POST: TeacherPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string TeacherFName, string TeacherLName, string EmployeeNumber, Double Salary)
        {
            Teacher UpdatedTeacher = new Teacher();
            UpdatedTeacher.FName = TeacherFName;
            UpdatedTeacher.LName = TeacherLName;
            UpdatedTeacher.ENo = EmployeeNumber;
            UpdatedTeacher.Salary = Salary;
            

            // not doing anything with the response
            _api.UpdateTeacher(id, UpdatedTeacher);
            // redirects to show author
            return RedirectToAction("Show", new { id = id });
        }
    }
}
