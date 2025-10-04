using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentScorecard.Models;
using System.Diagnostics;

namespace StudentScorecard.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentMarksContext _dbcontext;
        public HomeController(StudentMarksContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public ActionResult Index()
        {
            
            var students = _dbcontext.Students.ToList();
            return View(students);
        }

        public ActionResult Create()
        {
            var model = new StudentViewModels
            {
                AvailableSubjects = _dbcontext.Subjects.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(StudentViewModels model)
        {
            var student = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Class = model.Class,
                NumberofSubjects = model.SelectedSubjects.Count,
            };
            _dbcontext.Students.Add(student);
            _dbcontext.SaveChanges();
            foreach (var subid in model.SelectedSubjects)
            {
                var studentSubject = new StudentSubject
                {
                    StudentId = student.Id,
                    SubjectId = subid
                };
                _dbcontext.StudentSubjects.Add(studentSubject);
            }
            _dbcontext.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult AddMarks(int id)
        {
            var student = _dbcontext.Students.FirstOrDefault(s => s.Id == id);
            var subjects = _dbcontext.StudentSubjects.Where(s => s.StudentId == id).Select(s => new MarksItem
            {
                StudentSubjectId = s.Id,
                SubjectName = s.Subject.Name,
                Marks = s.Marks
            }).ToList();
            var vm = new MarksEntryViewModel
            {
                StudentId = student.Id,
                StudentName = student.FirstName + " " + student.LastName,
                Subjects = subjects
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult AddMarks(MarksEntryViewModel vm)
        {
            foreach (var item in vm.Subjects)
            {
                var dbSub = _dbcontext.StudentSubjects.First(s => s.Id == item.StudentSubjectId);
                dbSub.Marks = item.Marks;
            }
            _dbcontext.SaveChanges();

            var student = _dbcontext.Students
                .Include(s => s.StudentSubjects)
                .First(s => s.Id == vm.StudentId);

            int totalMarks = student.StudentSubjects.Sum(s => s.Marks ?? 0);
            int subjectCount = student.StudentSubjects.Count;
            student.Percentage = (double)totalMarks / (subjectCount * 100) * 100;

            _dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
