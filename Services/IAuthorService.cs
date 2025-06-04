using lib.Models;

namespace lib.Services;

public interface IAuthorService
{
    Task<ServiceResult<List<Author>>> GetAuthorsAsync();
    Task<ServiceResult<Author?>> GetAuthorByIdAsync(int id);
    Task<ServiceResult> StoreAsync(Author author);
    Task<ServiceResult> UpdateAsync(Author author);
    Task<ServiceResult> DeleteByIdAsync(int id);
}
