﻿using Microsoft.AspNetCore.Mvc;
using Study.DataAccess.Repository.IRepository;
using Study.Models;
using System.Diagnostics;

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
        public IActionResult Telefon()
        {
            IEnumerable<Product> productListTelefon = _unitOfWork.Products.GetAll().Where(u => u.ProductCategoryId == 11);
            return View(productListTelefon);
        }
        public IActionResult Tablet()
        {
            IEnumerable<Product> productListTablet = _unitOfWork.Products.GetAll().Where(u => u.ProductCategoryId == 13);
            return View(productListTablet);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}