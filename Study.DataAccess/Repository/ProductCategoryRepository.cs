using Study.DataAccess.Repository.IRepository;
using Study.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
