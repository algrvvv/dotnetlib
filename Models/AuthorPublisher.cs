using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("author_publishers")]
public class AuthorPublisher
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("author_id")]
    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;

    [Column("publisher_id")]
    public int PublisherId { get; set; }

    public Publisher Publisher { get; set; } = null!;
}
