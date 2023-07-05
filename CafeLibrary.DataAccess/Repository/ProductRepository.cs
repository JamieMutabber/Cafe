using CafeLibrary.DataAccess.Data;
using CafeLibrary.DataAccess.Repository.IRepository;
using CafeLibrary.Models;

namespace CafeLibrary.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
              
        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
