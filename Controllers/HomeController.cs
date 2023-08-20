using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WatchApp.Data;
using WatchApp.Models;
using WatchApp.Models.ViewModel;
using WatchApp.Services;

namespace WatchApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WatchDbContext _watchDbContext;

        public HomeController(ILogger<HomeController> logger, WatchDbContext watchDbContext)
        {
            _logger = logger;
            _watchDbContext = watchDbContext;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Watches = _watchDbContext.Watches.Include(i => i.Category).Include(i => i.ApplicationType),
                Categories = _watchDbContext.Categories
            };

            return View(homeViewModel);
        }

        public IActionResult Details(int id)
        {
            List<ShopingCard> shopingCards = new List<ShopingCard>();
            if (HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart).Count() > 0)
            {
                shopingCards = HttpContext.Session.Get<List<ShopingCard>>(WC.SessionCart);
            }

            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Watchs = _watchDbContext.Watches.Include(i => i.Category).Include(i => i.ApplicationType)
                .Where(i => i.Id == id).FirstOrDefault(),
                ExistsCard = false
            };

            foreach(var item in shopingCards)
            {
                if(item.WatchId == id)
                {
                    detailsViewModel.ExistsCard = true;
                }
            }

            return View(detailsViewModel);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShopingCard> shopingCards = new List<ShopingCard>();
            if(HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart).Count() > 0)
            {
                shopingCards = HttpContext.Session.Get<List<ShopingCard>>(WC.SessionCart);
            }

            shopingCards.Add(new ShopingCard { WatchId = id });
            HttpContext.Session.Set(WC.SessionCart, shopingCards);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveCard(int id)
        {
            List<ShopingCard> shopingCards = new List<ShopingCard>();
            if (HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShopingCard>>(WC.SessionCart).Count() > 0)
            {
                shopingCards = HttpContext.Session.Get<List<ShopingCard>>(WC.SessionCart);
            }

            var itemRemove = shopingCards.SingleOrDefault(i => i.WatchId == id);
            if(itemRemove != null)
            {
                shopingCards.Remove(itemRemove);
            }

            HttpContext.Session.Set(WC.SessionCart, shopingCards);
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
