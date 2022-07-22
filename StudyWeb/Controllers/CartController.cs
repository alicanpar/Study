using Microsoft.AspNetCore.Mvc;
using Study.DataAccess.Repository.IRepository;
using Study.Models.ViewModels;
using Study.Utility;

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
            var sessionCartId = HttpContext.Session.GetString("cartId");
            Guid cartId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartId = Guid.NewGuid();
                HttpContext.Session.SetString("cartId", cartId.ToString());
            }
            else
            {
                cartId = Guid.Parse(sessionCartId);
            }
            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCarts.GetAll(u => u.SessionGuid == cartId, includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        public IActionResult Plus(int cartId)
        {
            var sessionCartId = HttpContext.Session.GetString("cartId");
            Guid cartGuidId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartGuidId = Guid.NewGuid();
                HttpContext.Session.SetString("cartId", cartGuidId.ToString());
            }
            else
            {
                cartGuidId = Guid.Parse(sessionCartId);
            }
            var cart = _unitOfWork.ShoppingCarts.GetFirstOrDefault(u => u.SessionGuid == cartGuidId && u.Id == cartId);
            _unitOfWork.ShoppingCarts.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var sessionCartId = HttpContext.Session.GetString("cartId");
            Guid cartGuidId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartGuidId = Guid.NewGuid();
                HttpContext.Session.SetString("cartId", cartGuidId.ToString());
            }
            else
            {
                cartGuidId = Guid.Parse(sessionCartId);
            }
            var cart = _unitOfWork.ShoppingCarts.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCarts.Remove(cart);
                TempData["success"] = "Ürün Silindi";
                var count = _unitOfWork.ShoppingCarts.GetAll(u => u.SessionGuid == cartGuidId && u.Id == cartId).ToList().Count - 1;
                HttpContext.Session.SetInt32(SD.SessionCart, count);
            }
            else
            {
                _unitOfWork.ShoppingCarts.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var sessionCartId = HttpContext.Session.GetString("cartId");
            Guid cartGuidId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartGuidId = Guid.NewGuid();
                HttpContext.Session.SetString("cartId", cartGuidId.ToString());
            }
            else
            {
                cartGuidId = Guid.Parse(sessionCartId);
            }
            var cart = _unitOfWork.ShoppingCarts.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCarts.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCarts.GetAll(u => u.SessionGuid == cartGuidId && u.Id == cartId).ToList().Count;
            HttpContext.Session.SetInt32(SD.SessionCart, count);
            TempData["success"] = "Ürün Silindi!";
            return RedirectToAction(nameof(Index));
        }
    }
}
