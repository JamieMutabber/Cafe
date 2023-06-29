using CafeLibrary.DataAccess.Data;
using CafeLibrary.DataAccess.Repository.IRepository;
using CafeLibrary.Models;


namespace CafeLibrary.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }       

        public void Update(Category category)
        {
            _db.Categories.Update(category);
        }
    }
}
