using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAPI_Project.DTO;
using WebAPI_Project.Models;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "Return list of all departments",
            Description = "Get all departments with the number of students in each department",
            OperationId = "GetAllDepartments"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Received all departments", typeof(List<DepartmentDTO>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The department data is invalid")]

        public IActionResult GetAllDepartments()
        {
            List<Department> departments = db.Departments.Include(d => d.Students).ToList();
            List<DepartmentDTO> departmentDTOs = new List<DepartmentDTO>();
            foreach (Department Dept in departments)
            {
                DepartmentDTO departmentDTO = new DepartmentDTO()
                {
                    Id = Dept.Dept_Id,
                    Name = Dept.Dept_Name,
                    Location = Dept.Dept_Location,
                    //StudentsNames = Dept.Students.Select(n => n.St_Fname).ToList()
                };
                departmentDTOs.Add(departmentDTO);
            }
            return Ok(departmentDTOs);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get department by ID",
            Description = "Get department details by its ID",
            OperationId = "GetDepartmentById"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Received department details", typeof(DepartmentDTO))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Department not found")]

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
                    //departmentDTO.StudentsNames.Add(s.St_Fname);
                }
                return Ok(departmentDTO);
            }
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.Dept_Id)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            db.Departments.Add(department);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = department.Dept_Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await db.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Any(e => e.Dept_Id == id);
        }
    }

}
