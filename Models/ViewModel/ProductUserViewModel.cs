namespace WatchApp.Models.ViewModel
{
    public class ProductUserViewModel
    {
        public ProductUserViewModel()
        {
            WatchList = new List<Watch>();
        }

        public User UserApplication { get; set; }
        public IEnumerable<Watch> WatchList { get; set; }

    }
}
