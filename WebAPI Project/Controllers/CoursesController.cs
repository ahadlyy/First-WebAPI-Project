using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Project.Models;

namespace WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        ITIContext db;
        public CoursesController(ITIContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = db.Courses.ToList();
            if (courses == null || courses.Count() == 0)
            {
                return NotFound();
            }
            return Ok(courses);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCourseById(int id)
        {
            var course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpGet("{name:alpha}")]
        public IActionResult GetCourseByName(string crsname)
        {
            var course = db.Courses.FirstOrDefault(c=>c.Crs_Name == crsname);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            if (course == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            db.Courses.Add(course);
            db.SaveChanges();
            return CreatedAtAction("GetCourseById", new { id = course.Crs_Id }, course);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = db.Courses.SingleOrDefault(x => x.Crs_Id == id);
            if (course == null)
            {
                return NotFound();
            }
            db.Courses.Remove(course);
            db.SaveChanges();
            return Ok(course);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, Course c)
        {
            if (c == null)
            {
                return NotFound();
            }
            if (id != c.Crs_Id)
            {
                return BadRequest();
            }
            var existingCourse = db.Courses.FirstOrDefault(course => course.Crs_Id == id);

            if (existingCourse == null)
            {
                return NotFound();
            }
            db.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return Ok(c);
        }
    }
}
