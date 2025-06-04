using lib.Models;

namespace lib.Services;

public interface IBookService
{
    Task<ServiceResult<List<Book>>> GetBooksAsync();
    Task<ServiceResult<Book>> GetBookByIdAsync(int id);
    Task<ServiceResult<BookModifyViewModel>> GetModifyVM();
    Task<ServiceResult<BookModifyViewModel>> GetModifyVM(int bookId);
    Task<ServiceResult> StoreAsync(Book book);
    Task<ServiceResult> UpdateAsync(int id, Book book);
    Task<ServiceResult> DeleteAsync(int id);
}
