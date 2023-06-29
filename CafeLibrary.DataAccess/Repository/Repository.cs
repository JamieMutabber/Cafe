using CafeLibrary.DataAccess.Data;
using CafeLibrary.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CafeLibrary.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> DbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.DbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter) //GET by ID
        {
            IQueryable<T> query = DbSet;
            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll() // GET all
        {
            IQueryable<T> query = DbSet;
            return query.ToList();
        }

        public void Remove(T entity) //DELETE
        {
            DbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity) //DELETE multiple data
        {
            DbSet.RemoveRange(entity);
        }
    }
}
