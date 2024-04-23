namespace WebAPI_Project.DTO
{
    public class StudentDepartmentDTO
    {
        public int St_Id { get; set; }
        public string St_name { get; set; }

        //public string St_Lname { get; set; }

        public string St_Address { get; set; }

        public int? St_Age { get; set; }

        public string Deptartment_Name { get; set; }

        public string? Supervisor_Name { get; set; }
        public DepartmentDTO Department_Name { get; internal set; }
    }
}
