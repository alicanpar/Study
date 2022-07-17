using Microsoft.AspNetCore.Mvc;
using Study.DataAccess.Repository.IRepository;
using Study.Models;

namespace StudyWeb.Controllers
{
    
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductCategoryList()
        {
            IEnumerable<ProductCategory> objProductCategoryList = _unitOfWork.ProductCategory.GetAll();
            return View(objProductCategoryList);
        }
        public IActionResult ProductCategoryCreate()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProductCategoryCreate(ProductCategory obj)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.ProductCategory.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Ürün kategorisi oluşturuldu";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult ProductCategoryDelete(int? id)
        {
            IEnumerable<ProductCategory> objProductCategoryList = _unitOfWork.ProductCategory.GetAll();
            return View(objProductCategoryList);
        }        
       // [HttpPost]
       // [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.ProductCategory.GetFirstOrDefault(u => u.Id == id);
            if(obj==null)
            {
                return NotFound();
            }
            _unitOfWork.ProductCategory.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Ürün Kategorisi Başarıyla Silindi";
            return RedirectToAction("Index");
        }

        //Get
        public IActionResult ProductCategoryUpdate()
        {
            IEnumerable<ProductCategory> objProductCategoryList = _unitOfWork.ProductCategory.GetAll();
            return View(objProductCategoryList);
        }
        public IActionResult ProductCategoryUpdated(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }
            var ProductCategoryFromDbFirst = _unitOfWork.ProductCategory.GetFirstOrDefault(u => u.Id == id);
            if(ProductCategoryFromDbFirst==null)
            {
                return NotFound();
            }
            return View(ProductCategoryFromDbFirst);
        }
        //POST
        [HttpPost]
        public IActionResult ProductCategoryUpdated(ProductCategory obj)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.ProductCategory.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Ürün Kategorisi Düzenleme Başarılı";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult ProductCreate()
        {
            return View();
        }

    }
}
