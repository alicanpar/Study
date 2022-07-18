using Microsoft.AspNetCore.Mvc;
using Study.DataAccess.Repository.IRepository;
using Study.Models.ViewModels;

namespace StudyWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ShoppingCartVM = new ShoppingCartVM();
            {
                //ListCart = _unitOfWork.ShoppingCarts.GetAll(u => u.Id);
                //orderHeader = new();
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
    }
}
