using lib.Models;

namespace lib.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task StoreAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
}
