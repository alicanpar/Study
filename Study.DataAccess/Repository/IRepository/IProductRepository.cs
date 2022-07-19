using Study.DataAccess.Repository.IRepository;
using Study.Models;
namespace Study.DataAccess.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}
