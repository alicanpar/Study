using Microsoft.AspNetCore.Mvc;
using Study.DataAccess.Repository.IRepository;
using System.Diagnostics;
using Study.Models;
using Study.Utility;

namespace StudyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Products.GetAll(includeProperties: "ProductCategory");
            return View(productList);
        }
        public IActionResult Aboutus()
        {
            return View();
        }
        public IActionResult NotFound()
        {
            return View();
        }
        public IActionResult Contactus()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Products.GetFirstOrDefault(u => u.Id == productId, includeProperties: "ProductCategory")
            };
            return View(cartObj);
        }
        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingCart)
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

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCarts.GetFirstOrDefault(u => u.SessionGuid == cartId && u.ProductId == shoppingCart.ProductId);
            if (cartFromDb == null)
            {
                shoppingCart.SessionGuid = cartId;
                _unitOfWork.ShoppingCarts.Add(shoppingCart);
                _unitOfWork.Save();
                TempData["success"] = "Ürün Sepete Eklendi";
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCarts.GetAll(u => u.SessionGuid == cartId).ToList().Count);
            }
            else
            {
                _unitOfWork.ShoppingCarts.IncrementCount(cartFromDb, shoppingCart.Count);
                TempData["success"] = "Ürün Sepete Eklendi";
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ProductFiltre(string id)
        {
            var filterCase = _unitOfWork.Products.GetAll().AsQueryable();
            IEnumerable<Product> productList;
            switch (id)
            {
                case "telefon":
                    productList = filterCase.Where(w => w.ProductCategoryId == 11);
                    break;

                case "tablet":
                    productList = filterCase.Where(w => w.ProductCategoryId == 13);
                    break;

                case "laptop":
                    productList = filterCase.Where(w => w.ProductCategoryId == 14);
                    break;
                case "telefon_samsung":
                    productList = filterCase.Where(w => w.Mark == "Samswng" && w.ProductCategoryId == 11);
                    break;
                case "telefon_apple":
                    productList = filterCase.Where(w => w.Mark == "Apple" && w.ProductCategoryId == 11);
                    break;
                case "tablet_samsung":
                    productList = filterCase.Where(w => w.Mark == "Samsung" && w.ProductCategoryId == 13);
                    break;
                case "tablet_apple":
                    productList = filterCase.Where(w => w.Mark == "Apple" && w.ProductCategoryId == 13);
                    break;
                case "tablet_asus":
                    productList = filterCase.Where(w => w.Mark == "Asus" && w.ProductCategoryId == 14);
                    break;

                default:
                    productList = filterCase.Where(w => w.Id < 0);
                    break;
            }
            return View(productList);
            //return NotFound();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}