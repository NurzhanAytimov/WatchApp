using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchApp.Data;
using WatchApp.Models;

namespace WatchApp.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly WatchDbContext _watchDbContext;

        public ApplicationTypeController(WatchDbContext watchDbContext)
        {
            _watchDbContext = watchDbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> typeList = _watchDbContext.ApplicationTypes;
            return View(typeList);
           
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            _watchDbContext.Add(applicationType);
            _watchDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var item = _watchDbContext.ApplicationTypes.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)   //добавление категории
        {
            if (ModelState.IsValid)
            {
                _watchDbContext.Update(applicationType);
                _watchDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationType);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var item = _watchDbContext.ApplicationTypes.Find(id);
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
            var item = _watchDbContext.ApplicationTypes.Find(id);
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
