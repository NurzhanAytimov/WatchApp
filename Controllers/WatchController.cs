using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Drawing.Printing;
using WatchApp.Data;
using WatchApp.Models;
using WatchApp.Models.ViewModel;

namespace WatchApp.Controllers
{
    [Authorize(Roles = WC.AdminRole)] //доступ только для админа
    public class WatchController : Controller
    {
        private readonly WatchDbContext _watchDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WatchController(WatchDbContext watchDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _watchDbContext = watchDbContext;            
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            IEnumerable<Watch> watchList = _watchDbContext.Watches;
            
            foreach(var item in watchList)
            {
                item.Category = _watchDbContext.Categories.FirstOrDefault(i => i.Id == item.CategoryId); //получение списка чаcов и связанных категории
                item.ApplicationType = _watchDbContext.ApplicationTypes.FirstOrDefault(a => a.Id == item.ApplicationTypeId);
            }
            return View(watchList);
        }



        public IActionResult Create()
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                watch = new Watch(),
                selectListItems = _watchDbContext.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                applicationTypeSelectList = _watchDbContext.ApplicationTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            return View(productViewModel);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productviewmodel)   //добавление категории
        {
           
           
                var files = HttpContext.Request.Form.Files; //сохраняем картинку
                string path = _webHostEnvironment.WebRootPath; //определение директории папки

                string upload = path + WC.ImagesPath;
                string fileName = Guid.NewGuid().ToString(); //задаем рандомное имя файлу
                string extencion = Path.GetExtension(files[0].FileName); //получение расширения файла

                using(var filestream = new FileStream(Path.Combine(upload, fileName + extencion), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                productviewmodel.watch.Image = fileName + extencion;

                _watchDbContext.Add(productviewmodel.watch);
                _watchDbContext.SaveChanges();
                return RedirectToAction("Index");
        }



        public IActionResult Edit(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                watch = new Watch(),

                selectListItems = _watchDbContext.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                applicationTypeSelectList = _watchDbContext.ApplicationTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return NotFound();
            }

            productViewModel.watch = _watchDbContext.Watches.Find(id);
            if (productViewModel.watch == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel productViewModel)   //добавление категории
        {
            var files = HttpContext.Request.Form.Files;
            string path = _webHostEnvironment.WebRootPath;
            var objectEdit = _watchDbContext.Watches.AsNoTracking().FirstOrDefault(i => i.Id == productViewModel.watch.Id);

            if(files.Count > 0)
            {
                string upload = path + WC.ImagesPath;
                string fileName = Guid.NewGuid().ToString(); //задаем рандомное имя файлу
                string extencion = Path.GetExtension(files[0].FileName);

                var deleteFile = Path.Combine(upload, objectEdit.Image); //получаем старое изоброжение

                if (System.IO.File.Exists(deleteFile)) //если изборожение есть, то удаляем его
                {
                    System.IO.File.Delete(deleteFile);
                }

                using (var filestream = new FileStream(Path.Combine(upload, fileName + extencion), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                productViewModel.watch.Image = fileName + extencion; //сохраняем ссылку на новое изоброжение
            }

            else
            {
                productViewModel.watch.Image = objectEdit.Image; //если изоброжение не поменялось, то остовляем его
            }

            _watchDbContext.Watches.Update(productViewModel.watch);
            _watchDbContext.SaveChanges();
            return RedirectToAction("Index");
      

        }


        //метод GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Watch watch = _watchDbContext.Watches.Include(i => i.Category).Include(i => i.ApplicationType).FirstOrDefault(i => i.Id == id);

            if (watch == null)
            {
                return NotFound();
            }

            return View(watch);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)   //удаление категории
        {

            var item = _watchDbContext.Watches.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagesPath;

            var deleteFile = Path.Combine(upload, item.Image); //получаем старое изоброжение

            if (System.IO.File.Exists(deleteFile)) //если изборожение есть, то удаляем его
            {
                System.IO.File.Delete(deleteFile);
            }

            _watchDbContext.Watches.Remove(item);
            _watchDbContext.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
