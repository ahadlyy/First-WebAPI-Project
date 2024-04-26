using WebAPI_Project.Models;

namespace WebAPI_Project.Repositories
{
    public class GenericRepo<TEntity> where TEntity : class
    {
        private readonly ITIContext db;
        public GenericRepo(ITIContext _db)
        {
            db = _db;
        }
        public List<TEntity> GetAll()
        {
            return db.Set<TEntity>().ToList();
        }
        public TEntity GetById(int id)
        {
            return db.Set<TEntity>().Find(id);
        }
        public void Add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity e)
        {
            //db.Entry(e).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.Set<TEntity>().Update(e);
        }
        public void Delete(int id)
        {
            db.Set<TEntity>().Remove(GetById(id));
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
