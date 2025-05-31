using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("publishers")]
public class Publisher
{
    /// <summary>
    /// айди автора
    /// </summary>    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// название издательства
    /// </summary>
    [Required(ErrorMessage = "Название издательства является обязательным параметром")]
    [MaxLength(255, ErrorMessage = "Максимальная длина названия издательства не должна превышать 255 символов")]
    [Column("name")]
    public required string Name { get; set; }

    /// <summary>
    /// краткое описание автора 
    /// </summary>
    [Required(ErrorMessage = "Описание издательства является обязательным параметром")]
    [MaxLength(255, ErrorMessage = "Максимальная длина описания издательства не должна превышать 255 символов")]
    [Column("description")]
    public required string Desc { get; set; }

    /// <summary>
    /// список привязок к автору, которые привязаны к издательству
    /// </summary>
    public List<AuthorPublisher> AuthorPublishers { get; set; } = new();

    public override string ToString()
    {
        return $"[Id: {Id}; Name: {Name}; Desc: {Desc};];";
    }
}
