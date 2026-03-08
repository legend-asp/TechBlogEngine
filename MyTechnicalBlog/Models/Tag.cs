using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTechnicalBlog.Models // ДОБАВЬ ЭТУ СТРОКУ
{
    [Table("Теги")]
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название тега обязательно")]
        [StringLength(20, ErrorMessage = "Тег не должен быть длиннее 20 символов")]
        public required string Name { get; set; }

        public List<Article> Articles { get; set; } = new();
    }
}