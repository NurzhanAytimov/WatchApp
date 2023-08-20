namespace WatchApp.Models.ViewModel
{
    public class HomeViewModel
    {
       public IEnumerable<Watch> Watches { get; set; }
       public IEnumerable<Category> Categories { get; set; }
    }
}
