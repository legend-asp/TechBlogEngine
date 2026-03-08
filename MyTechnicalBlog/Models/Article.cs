using MyTechnicalBlog.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Статьи")]
public class Article
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    // Делаем AuthorId необязательным (int?), чтобы БД не требовала его при сохранении
    public int? AuthorId { get; set; }
    public User? Author { get; set; }

    public List<Tag> Tags { get; set; } = new();
}