using WebAPI_Project.Models;
using WebAPI_Project.Repositories;

namespace WebAPI_Project.WorkUnit
{
    public class UnitOfWork
    {
        ITIContext db;
        GenericRepo<Student> stdRepo;
        GenericRepo<Department> deptRepo;
        public UnitOfWork(ITIContext _db)
        {
            db = _db;
        }
        public GenericRepo<Student> StdRepo
        {
            get
            {
                if (stdRepo == null)
                {
                    stdRepo = new GenericRepo<Student>(db);
                }
                return stdRepo;
            }
        }
        public GenericRepo<Department> DeptRepo
        {
            get
            {
                if (deptRepo == null)
                {
                    deptRepo = new GenericRepo<Department>(db);
                }
                return deptRepo;
            }
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
