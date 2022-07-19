using Study.DataAccess.IRepository;
using Study.Models;
namespace Study.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objProductFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objProductFromDb != null)
            {
                objProductFromDb.Name = obj.Name;
                objProductFromDb.SerialNumber = obj.SerialNumber;
                objProductFromDb.ImeiCode = obj.ImeiCode;
                objProductFromDb.Color = obj.Color;
                objProductFromDb.Mark = obj.Mark;
                objProductFromDb.Size = obj.Size;
                objProductFromDb.Price = obj.Price;
                objProductFromDb.ProductCategoryId = obj.ProductCategoryId;
                if (obj.ImageUrl != null)
                {
                    objProductFromDb.ImageUrl = obj.ImageUrl;
                }
            }

        }
        //{
        //    var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
        //    if(objFromDb != null)
        //    {
        //        tablo
        //        objfromdb.title = obj.title;
        //        if (obj.ImageUrl != null)
        //        {
        //            obj.FromDb.ImageUrl = obj.ImageUrl;
        //        }
        //    }
        //}
    }
}
