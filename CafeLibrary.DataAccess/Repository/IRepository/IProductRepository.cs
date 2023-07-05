using CafeLibrary.Models;

namespace CafeLibrary.DataAccess.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product product);
    }
}
