using Microsoft.AspNetCore.Mvc;
using Study.DataAccess.Repository.IRepository;
using Study.Models;
using Study.Utility;

namespace StudyWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
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


            if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
            {
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCarts.GetAll(u => u.SessionGuid == cartId).ToList().Count);
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
        }
    }
}
