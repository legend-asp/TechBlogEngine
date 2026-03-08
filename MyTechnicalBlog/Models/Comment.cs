using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Комментарии")]
public class Comment
{
    [Key]
    [Column("Идентификатор")]
    public int Id { get; set; }

    [Column("Текст_Комментария")]
    [Required(ErrorMessage = "Напишите текст комментария")]
    [StringLength(500, ErrorMessage = "Комментарий не может превышать 500 символов")]
    public required string Text { get; set; }

    [Column("Статья_Идентификатор")]
    public int ArticleId { get; set; }

    public required Article Article { get; set; }
}