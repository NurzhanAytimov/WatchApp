using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WatchApp.Data;
using WatchApp.Models;
using WatchApp.Models.ViewModel;
using WatchApp.Services;

namespace WatchApp.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly WatchDbContext _watchDbContext;
        [BindProperty]
        public ProductUserViewModel ProductUser { get; set; }
        public BasketController(WatchDbContext watchDbContext)
        {
            _watchDbContext = watchDbContext;
        }

        public IActionResult Index()
        {
            List<ShopingCard> shopingCardList = new List<ShopingCard>();

            if(HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart).Count() > 0)
            {
                shopingCardList = HttpContext.Session.Get<List<ShopingCard>>(WC.SessionCart); //получаем продукт из сессий
            }

            List<int> watchCartId = shopingCardList.Select(p => p.WatchId).ToList();
            IEnumerable<Watch> watches = _watchDbContext.Watches.Where(i => watchCartId.Contains(i.Id));

            return View(watches);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;  //вывод данных пользователя при покупке
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShopingCard> shopingCardList = new List<ShopingCard>();

            if (HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart).Count() > 0)
            {
                shopingCardList = HttpContext.Session.Get<List<ShopingCard>>(WC.SessionCart); //получаем продукт из сессий
            }

            List<int> watchCartId = shopingCardList.Select(p => p.WatchId).ToList();
            IEnumerable<Watch> watches = _watchDbContext.Watches.Where(i => watchCartId.Contains(i.Id));


            ProductUser = new ProductUserViewModel()
            {
                UserApplication = _watchDbContext.Users.FirstOrDefault(i => i.Id == claims.Value),
                WatchList = watches
            };

            return View(ProductUser);
        }


        public IActionResult Remove(int id)
        {
            List<ShopingCard> shopingCardList = new List<ShopingCard>();

            if (HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart).Count() > 0)
            {
                shopingCardList = HttpContext.Session.Get<List<ShopingCard>>(WC.SessionCart); //получаем продукт из сессий
            }

            shopingCardList.Remove(shopingCardList.FirstOrDefault(i => i.WatchId == id));

            HttpContext.Session.Set(WC.SessionCart, shopingCardList); //обновляем ссесию после удаление товара

            return RedirectToAction("Index");

        }
    }
}
