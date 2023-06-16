using GradeBook.EntityData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GradeBook.Controllers
{
    public class StudentController : Controller
    {
        EntityDB _entityDB;
        public StudentController(EntityDB entitydb)
        {
            this._entityDB = entitydb;
        }

      public IActionResult Dashboard()
        {
            return View();
        }
       
        [HttpGet]
        public IActionResult StudentIndex(string searchName, string sortOrder)
        {
            // Retrieve all students from the database
            IQueryable<Student> students = _entityDB.Students.AsQueryable();

            // Filter the students based on the search name
            if (!string.IsNullOrEmpty(searchName))
            {
                students = students.Where(s => s.Name.Contains(searchName));
            }

            // Apply sorting based on the sortOrder parameter
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                case "total_asc":
                    students = students.OrderBy(s => s.Total);
                    break;
                case "total_desc":
                    students = students.OrderByDescending(s => s.Total);
                    break;
                default:
                    students = students.OrderBy(s => s.Id);
                    break;
            }

            // Pass the filtered and sorted student list to the view
            return View(students.ToList());
        }


        [HttpPost]

        public IActionResult Create(Student student)
        {
            int maxId = _entityDB.Students.Any() ? _entityDB.Students.Max(s => s.Roll_No) : 0;

            // Increment the maximum ID value
            int newId = maxId + 1;

            // Assign the new ID value to the student object
            student.Roll_No = newId;
            student.CalculateTotal();
            student.CalculateGrade();

            _entityDB.Students.Add(student);
            _entityDB.SaveChanges();

            return RedirectToAction("StudentIndex");
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Marksheet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _entityDB.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        public IActionResult Edit(int id)
        {
            var student = _entityDB.Students.Where(x => x.Id == id).FirstOrDefault();
            return View(student);
        }
     
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            student.CalculateTotal();
            student.CalculateGrade();
            _entityDB.Attach(student);
            _entityDB.Students.Entry(student).State = EntityState.Modified;
            _entityDB.SaveChanges();
            return RedirectToAction("StudentIndex");
        }
        public IActionResult Delete(int id)
        {
            var studentToDelete = _entityDB.Students.FirstOrDefault(x => x.Id == id);
            if (studentToDelete == null)
            {
                return NotFound();
            }

            var studentsToUpdate = _entityDB.Students.Where(x => x.Roll_No > studentToDelete.Roll_No).ToList();

            _entityDB.Students.Remove(studentToDelete);

            // Update the roll numbers for the remaining students
            foreach (var student in studentsToUpdate)
            {
                student.Roll_No -= 1;
            }

            _entityDB.SaveChanges();

            return RedirectToAction("StudentIndex");
        }

    }
}

