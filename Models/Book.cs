using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("books")]
public class Book
{
    // айди книги
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    // колво страниц в книге
    [Required(ErrorMessage = "Количество страниц в книге является обязательным параметром")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество страниц должно быть положительным")]
    [Column("page_count")]
    public int PageCount { get; set; }

    // название книги
    [Required(ErrorMessage = "Название книги является обязательным параметром")]
    [Column("title")]
    public string? Title { get; set; }

    // привязка к автору
    [Required(ErrorMessage = "Автор книги является обязательным параметром")]
    [Column("author_id")]
    public int AuthorId { get; set; }

    // автор книги
    public Author? Author { get; set; }

    public override string ToString()
    {
        return $"[Id: {Id}; Title: {Title}; PageCount: {PageCount}; AuthorId: {AuthorId}]";
    }
}

