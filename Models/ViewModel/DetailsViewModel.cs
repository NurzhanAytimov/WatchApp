namespace WatchApp.Models.ViewModel
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Watchs = new Watch();
        }
        public Watch Watchs { get; set; }
        public bool ExistsCard { get; set; }
    }
}
