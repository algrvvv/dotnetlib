using System.ComponentModel.DataAnnotations;

namespace lib.Models;

public class PublisherAuthorViewModel
{
    public Publisher Publisher { get; set; } = null!;

    [Required(ErrorMessage = "Список авторов является обязательным")]
    public List<int> SelectedAuthors { get; set; } = new();

    public List<Author> AuthorsList { get; set; } = new();

    public override string ToString()
    {
        return $"[Publisher.Name: {Publisher.Name}; Publisher.Desc: {Publisher.Desc}; SelectedAuthors: {SelectedAuthors}]";
    }
}
