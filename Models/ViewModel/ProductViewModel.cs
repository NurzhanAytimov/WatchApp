using Microsoft.AspNetCore.Mvc.Rendering;

namespace WatchApp.Models.ViewModel
{
    public class ProductViewModel
    {
       public Watch? watch { get; set; }
       public IEnumerable<SelectListItem> selectListItems { get; set; }
        public IEnumerable<SelectListItem> applicationTypeSelectList { get; set; }
    }
}
