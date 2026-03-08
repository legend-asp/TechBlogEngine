using MyTechnicalBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Роли")]
public class Role
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Название роли обязательно")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "Роль должна быть от 2 до 30 символов")]
    public required string Name { get; set; }

    public List<User> Users { get; set; } = new();
}