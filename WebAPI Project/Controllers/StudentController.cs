using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]

        public List<Student> GetallStudents() 
        {
            return db.Students.ToList();
        }

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
