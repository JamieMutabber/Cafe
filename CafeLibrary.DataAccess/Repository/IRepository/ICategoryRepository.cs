using CafeLibrary.Models;


namespace CafeLibrary.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
       
    }
}
