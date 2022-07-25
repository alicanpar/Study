using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Study.DataAccess.Repository.IRepository;
using Study.Models;
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
            var sessionCartId = HttpContext.Request.Cookies["cartId"];
            Guid cartId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartId = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("cartId", cartId.ToString());
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
        [HttpGet]
        public IActionResult Summary()
        {
            var sessionCartId = HttpContext.Request.Cookies["cartId"];
            Guid cartId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartId = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("cartId", cartId.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(3) });
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
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartVM model)
        {
            var sessionCartId = HttpContext.Request.Cookies["cartId"];
            Guid cartId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartId = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("cartId", cartId.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(3) });
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
            ShoppingCartVM.OrderHeader = model.OrderHeader;
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            _unitOfWork.OrderHeaders.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetail);
                _unitOfWork.Save();
            }
            //stripe ayarları
            var domain = "https://localhost:44354/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"Cart/Index",
            };
            foreach (var item in ShoppingCartVM.ListCart)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "try",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeaders.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeaders.GetFirstOrDefault(u => u.Id == id);
            //OrderDetail orderDetail = _unitOfWork.OrderDetails.GetFirstOrDefault(u => u.Id == id);
            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeaders.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCarts.GetAll(u => u.Id == id).ToList();
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete("cartId");
            _unitOfWork.ShoppingCarts.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }

        public IActionResult Plus(int cartId)
        {
            var sessionCartId = HttpContext.Request.Cookies["cartId"];
            Guid cartGuidId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartGuidId = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("cartId", cartGuidId.ToString());
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
            var sessionCartId = HttpContext.Request.Cookies["cartId"];
            Guid cartGuidId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartGuidId = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("cartId", cartGuidId.ToString());
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
            var sessionCartId = HttpContext.Request.Cookies["cartId"];
            Guid cartGuidId;
            if (string.IsNullOrEmpty(sessionCartId))
            {
                cartGuidId = Guid.NewGuid();
                HttpContext.Response.Cookies.Append("cartId", cartGuidId.ToString());
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
