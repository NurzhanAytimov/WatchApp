using System.ComponentModel.DataAnnotations;

namespace WatchApp.Models
{
    public class ApplicationType
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Поле обязательно должно быть заполнено")]
        public string Name { get; set; }
    }
}
