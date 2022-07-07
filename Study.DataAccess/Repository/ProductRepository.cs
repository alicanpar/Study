using Study.DataAccess.IRepository;
using Study.DataAccess.Repository.IRepository;
using Study.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
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
