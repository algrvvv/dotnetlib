namespace lib.Models;

public class BookModifyViewModel
{

    public Book Book { get; set; } = new();

    public List<Author> Authors { get; set; } = new();
}
