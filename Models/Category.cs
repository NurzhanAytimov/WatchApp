using MessagePack;
using System.ComponentModel.DataAnnotations;


namespace WatchApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Поле обязательное для заполнения")]
        public string Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage ="Порядок заполнение должен начинатся с 1")]
        [Required(ErrorMessage = "Поле обязательное для заполнения")]
        public int DisplayOrder { get; set; }
    }
}
