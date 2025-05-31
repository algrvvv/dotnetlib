using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("authors")]
public class Author
{
    /// <summary>
    /// айди автора
    /// </summary>    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// фио автора
    /// </summary>
    [Required(ErrorMessage = "ФИО автора является обязательным параметром")]
    [Column("full_name")]
    public required string FullName { get; set; }

    /// <summary>
    /// краткое описание автора 
    /// </summary>
    [Required(ErrorMessage = "Описание автора является обязательным параметром")]
    [Column("description")]
    public required string Desc { get; set; }

    /// <summary>
    ///  список книг, которые написал автор
    /// </summary>
    public List<Book>? Books { get; set; } = new();

    /// <summary>
    /// список издательств, к которым привязан автор
    /// </summary>
    public List<AuthorPublisher> AuthorPublishers { get; set; } = new();

    public override string ToString()
    {
        return $"[Id: {this.Id}; FullName: {this.FullName}; Desc: {this.Desc};];";
    }
}
