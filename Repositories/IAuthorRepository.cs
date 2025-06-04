using lib.Models;

namespace lib.Repositories;

public interface IAuthorRepository
{
    Task<List<Author>> GetAuthorsAsync();
    Task<Author?> GetAuthorByIdAsync(int id);
    Task StoreAsync(Author author);
    Task UpdateAsync(Author author);
    Task DeleteAsync(int id);
}
