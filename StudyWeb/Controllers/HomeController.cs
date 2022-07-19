using Microsoft.AspNetCore.Mvc;
using Study.DataAccess.Repository.IRepository;
using System.Diagnostics;
using Study.Models;
using System.Security.Claims;

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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCarts.GetFirstOrDefault(u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);
            if(cartFromDb == null)
            {
                _unitOfWork.ShoppingCarts.Add(shoppingCart);
                _unitOfWork.Save();
                TempData["success"] = "Ürün Sepete Eklendi";
                //HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCarts.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
            }
            else
            {
                _unitOfWork.ShoppingCarts.IncrementCount(cartFromDb, shoppingCart.Count);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ProductFiltre(string id)
        {
            if (id == "telefon")
            {//telefon
                IEnumerable<Product> productList = _unitOfWork.Products.GetAll().Where(u => u.ProductCategoryId == 11);
                return View(productList);
            }
            else if (id == "tablet")
            {//tablet
                IEnumerable<Product> productList = _unitOfWork.Products.GetAll().Where(u => u.ProductCategoryId == 13);
                return View(productList);
            }
            else if(id=="telefon_samsung")
            {//telefon ve samsung
                IEnumerable<Product> productList = _unitOfWork.Products.GetAll().Where(u => u.Mark == "Samsung" && u.ProductCategoryId==11);
                return View(productList);
            }
            else if (id == "telefon_apple")
            {//telefon ve apple
                IEnumerable<Product> productList = _unitOfWork.Products.GetAll().Where(u => u.Mark == "Apple" && u.ProductCategoryId==11);
                return View(productList);
            }
            else if (id == "tablet_samsung")
            {//tablet ve samsung
                IEnumerable<Product> productList = _unitOfWork.Products.GetAll().Where(u => u.Mark == "Samsung" && u.ProductCategoryId == 13);
                return View(productList);
            }
            else if (id == "tablet_apple")
            {//tablet ve apple
                IEnumerable<Product> productList = _unitOfWork.Products.GetAll().Where(u => u.Mark == "Apple" && u.ProductCategoryId == 13);
                return View(productList);
            }
            return NotFound();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}