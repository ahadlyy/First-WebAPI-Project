using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WebAPI_Project.DTO;
using WebAPI_Project.Models;

namespace WebAPI_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        ITIContext db;
        public DepartmentController(ITIContext db)
        {
            this.db = db;
        }

        [HttpGet]

        public IActionResult GetAllDepartments()
        {
            List<Department> departments = db.Departments.Include(d => d.Students).ToList();
            List<DepartmentDTO> departmentDTOs = new List<DepartmentDTO>(); 
            foreach(Department Dept in departments)
            {
                DepartmentDTO departmentDTO = new DepartmentDTO()
                {
                    Id = Dept.Dept_Id,
                    Name = Dept.Dept_Name,
                    Location = Dept.Dept_Location,
                    StudentsNames = Dept.Students.Select(n => n.St_Fname).ToList()
                };
                departmentDTOs.Add(departmentDTO);
            }
            return Ok(departmentDTOs);
        }

        [HttpGet("{id}")]

        public IActionResult GetDeptById(int id)
        {
            Department d = db.Departments.Include(d => d.Students).FirstOrDefault(d => d.Dept_Id == id);
            if (d == null)
            {
                return NotFound();
            }
            else
            {
                DepartmentDTO departmentDTO = new DepartmentDTO()
                {
                    Id = d.Dept_Id,
                    Name = d.Dept_Name,
                    Location = d.Dept_Location
                };
                foreach (Student s in d.Students)
                {
                    departmentDTO.StudentsNames.Add(s.St_Fname);
                }
                return Ok(departmentDTO);
            }
        }

    }
}
