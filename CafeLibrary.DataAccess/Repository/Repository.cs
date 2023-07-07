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

            //adding Category table and categoryID to Product Table (RelationShip)
            _context.Products.Include(u => u.Id).Include(u => u.CategoryId);
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public T Get(System.Linq.Expressions.Expression<Func<T, bool>> filter, string? includeProperties = null) //GET by ID
        {
            IQueryable<T> query = DbSet;
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperties = null) // GET all
        {
            IQueryable<T> query = DbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries ))
                {
                    query = query.Include(includeProp);
                }
            }
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
