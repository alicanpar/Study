using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Study.DataAccess.Repository.IRepository;
using Study.Models;
using Study.Models.ViewModels;

namespace StudyWeb.Controllers;

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }
    public IActionResult Index()
    {
        return View();
    }
    //GET
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            ProductCategoryList = _unitOfWork.ProductCategory.GetAll().Select(i => new SelectListItem
            {
                Text = i.Type,
                Value = i.Id.ToString()
            })
        };
        if (id == null || id == 0)
        {
            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.Products.GetFirstOrDefault(u => u.Id == id);
            return View(productVM);
            //update product
        }
    }
    //POST
    [HttpPost]
    public IActionResult Upsert(ProductVM obj, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            string wwwRoothPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRoothPath, @"images\products");
                if (obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRoothPath, obj.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                var extension = Path.GetExtension(file.FileName);
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }
            if (obj.Product.Id == 0)
            {
                _unitOfWork.Products.Add(obj.Product);
            }
            else
            {
                _unitOfWork.Products.Update(obj.Product);
            }
            _unitOfWork.Save();
            TempData["success"] = "Ürün Başarıyla Oluşturuldu";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Products.GetAll(includeProperties: "ProductCategory");
        return Json(new { data = productList });
    }

    //Post
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Products.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Silme Başarısız" });
        }
        var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        _unitOfWork.Products.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Silme Başarılı" });
    }
    #endregion
}

