using Study.DataAccess.IRepository;
namespace Study.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductCategoryRepository ProductCategory { get; }
        IProductRepository Products { get; }
        IShoppingCartRepository ShoppingCarts { get; }
        IOrderHeaderRepository OrderHeaders { get; }
        IOrderDetailRepository OrderDetails { get; }
        //IApplicationUserRepository ApplicationUsers { get; }
        void Save();
    }
}
