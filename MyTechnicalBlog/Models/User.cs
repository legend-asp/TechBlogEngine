using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTechnicalBlog.Models;

[Table("Пользователи")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите имя пользователя")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Имя должно быть от 3 до 20 символов")]
    [Display(Name = "Имя пользователя")]
    public required string Username { get; set; }

    public List<Article> Articles { get; set; } = new();
    public List<Role> Roles { get; set; } = new();
}