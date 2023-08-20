using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchApp.Data;
using WatchApp.Models;

namespace WatchApp.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly WatchDbContext _watchDbContext;
        public CategoryController(WatchDbContext watchDbContext)
        {
            _watchDbContext = watchDbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _watchDbContext.Categories;
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)   //добавление категории
        {
            if (ModelState.IsValid)
            {
                _watchDbContext.Add(category);
                _watchDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
           
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var item = _watchDbContext.Categories.Find(id);
            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)   //добавление категории
        {
            if (ModelState.IsValid)
            {
                _watchDbContext.Update(category);
                _watchDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);

        }
        //метод GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var item = _watchDbContext.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)   //удаление категории
        {
            var item = _watchDbContext.Categories.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            _watchDbContext.Remove(item);
            _watchDbContext.SaveChanges();
            return RedirectToAction("Index");
           
        }

    }
}
