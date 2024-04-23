using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Project.DTO;
using WebAPI_Project.Models;

namespace WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        ITIContext db;
        public StudentsController(ITIContext db)
        {
            this.db = db;
        }

        [HttpGet]

        public IActionResult GetallStudents()
        {
            List<Student> stu = db.Students.ToList();
            List<StudentDTO> stuDTO = new List<StudentDTO>();
            foreach (Student s in stu)
            {
                StudentDTO sDTO = new StudentDTO()
                {
                    Id = s.St_Id,
                    Fname = s.St_Fname,
                    Adderss = s.St_Address,
                    Age = s.St_Age
                };   
                stuDTO.Add(sDTO);
            }
            return Ok(stuDTO);
        }

        [HttpGet("{id:int}")]

        public IActionResult Getstudentbyid(int id)
        {
            Student st = db.Students.Find(id);
            if (st == null)
            {
                return NotFound();
            }
            else
            {
                StudentDTO studentDTO = new StudentDTO()
                {
                    Id = st.St_Id,
                    Fname = st.St_Fname,
                    Adderss = st.St_Address,
                    //DepartmentName = st.Dept.Dept_Name,
                    Age = st.St_Age
                };
                return Ok(studentDTO);
            }
        }
    }
}
