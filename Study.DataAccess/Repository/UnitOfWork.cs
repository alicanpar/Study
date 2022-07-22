using Study.DataAccess.IRepository;
using Study.DataAccess.Repository.IRepository;
namespace Study.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Products = new ProductRepository(_db);
            ProductCategory = new ProductCategoryRepository(_db);
            ShoppingCarts = new ShoppingCartRepository(_db);
            OrderDetails = new OrderDetailRepository(_db);
            OrderHeaders = new OrderHeaderRepository(_db);
            //ApplicationUsers = new ApplicationUserRepository(_db);
        }
        public IProductCategoryRepository ProductCategory { get; private set; }
        public IProductRepository Products { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }
        public IOrderHeaderRepository OrderHeaders { get; private set; }
        public IShoppingCartRepository ShoppingCarts { get; private set; }
        //public IApplicationUserRepository ApplicationUsers { get; private set; }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
