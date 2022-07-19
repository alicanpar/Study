using Study.DataAccess.Repository.IRepository;
using Study.Models;
namespace Study.DataAccess.Repository
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private ApplicationDbContext _db;
        public ProductCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ProductCategory obj)
        {
            _db.ProductCategories.Update(obj);
        }
    }
}
