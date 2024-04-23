using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Project.DTO;
using WebAPI_Project.Models;

namespace WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        ITIContext db;
        public StudentController(ITIContext db)
        {
            this.db = db;
        }
        /// <summary>
        /// get student all students
        /// </summary>
        /// <param name="page"> page number</param>
        /// <param name="limit"> page limit </param>
        /// <returns> list of students</returns>
        /// <remarks>
        /// request example:
        ///  /api/student
        /// </remarks>


        [HttpGet]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            var students = db.Students.Include(a => a.St_superNavigation).Include(a => a.Dept).ToList();

            if (students == null || students.Count == 0)
                return NotFound("There are no students.");

            int totalPages = (int)Math.Ceiling((double)students.Count / limit);

            if (page > totalPages)
                return BadRequest("There is no page with that number.");

            var result = students.Skip((page - 1) * limit).Take(limit);

            var studentsDTOList = result.Select(s => CreateDTO(s)).ToList();

            return Ok(studentsDTOList);
        }

        [NonAction]
        private StudentDepartmentDTO CreateDTO(Student student)
        {
            return new StudentDepartmentDTO
            {
                St_Id = student.St_Id,
                St_name = student.St_Fname,
                St_Age = student.St_Age,
                Department_Name = new DepartmentDTO
                {
                    Id = student.Dept.Dept_Id,
                    Name = student.Dept.Dept_Name,
                    Location = student.Dept.Dept_Location
                }
            };
        }


        //[HttpGet]

        //public List<Student> GetallStudents() 
        //{
        //    return db.Students.ToList();
        //}

        [HttpGet("{id:int}")]

        public IActionResult Getstudentbyid(int id) 
        {
            Student st = db.Students.Find(id);
            if(st == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(st);
            }
        }

        [HttpGet("/api/std/{fname}")]
        //[HttpGet("{fname:alpha}")]

        public IActionResult Getstudentfname(string fname)
        {
            Student st = db.Students.FirstOrDefault(n => n.St_Fname == fname);
            if(st == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(st);
            }
        }

        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]

        public IActionResult addstudent(Student st) 
        {
            if(st == null)
            {
                return BadRequest("please enter the required data");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("please enter correct data");
            }
            db.Students.Add(st);
            db.SaveChanges();
            return CreatedAtAction("Getstudentbyid",new {id = st.St_Id},st); // returns url for the added student
        }

        [HttpPut("{id}")]

        public IActionResult editstudent (Student st,[FromRoute] int id) //atribute to spcify from where get the data
        {
            if(st == null)
            {
                return BadRequest();
            }
            if(st.St_Id != id) 
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            db.Entry(st).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return NoContent();
        }

        [HttpDelete]

        public IActionResult deletestudent(int id)
        {
            Student st = db.Students.Find(id);
            if(st == null)
            {
                return NotFound();
            }
            else
            {
                db.Students.Remove(st);
                db.SaveChanges();
                return Ok(st);
            }
        }
    }
}
