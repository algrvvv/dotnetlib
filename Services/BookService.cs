using lib.Models;
using lib.Repositories;

namespace lib.Services;

public class BookService : IBookService
{
    private readonly ILogger<BookService> _logger;
    private readonly IBookRepository _repo;
    private readonly IAuthorRepository _authorRepo;

    public BookService(ILogger<BookService> logger, IBookRepository repo, IAuthorRepository authorRepo)
    {
        _logger = logger;
        _repo = repo;
        _authorRepo = authorRepo;
    }

    public async Task<ServiceResult<List<Book>>> GetBooksAsync()
    {
        try
        {
            var books = await _repo.GetBooksAsync();
            return ServiceResult<List<Book>>.Ok(books, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get books");
            return ServiceResult<List<Book>>.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult<Book>> GetBookByIdAsync(int id)
    {
        try
        {
            var book = await _repo.GetBookByIdAsync(id);
            return ServiceResult<Book>.Ok(book, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get book by id: {Id}", id);
            return ServiceResult<Book>.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult<BookModifyViewModel>> GetModifyVM()
    {
        try
        {
            var authors = await _authorRepo.GetAuthorsAsync();
            _logger.LogInformation("got authors for modify book view model. count: {Count}", authors.Count);

            var vm = new BookModifyViewModel
            {
                Authors = authors
            };
            return ServiceResult<BookModifyViewModel>.Ok(vm, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get book modify book view model");
            return ServiceResult<BookModifyViewModel>.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult> StoreAsync(Book book)
    {
        try
        {
            await _repo.StoreAsync(book);
            return ServiceResult.Ok("Книга успешно добавлена");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to store new book");
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }

    }

    public async Task<ServiceResult<BookModifyViewModel>> GetModifyVM(int bookId)
    {
        try
        {
            // получаем доступных список авторов для привязки к книге
            var authors = await _authorRepo.GetAuthorsAsync();
            _logger.LogInformation("got authors for edit book. count: {Count}", authors.Count);

            var book = await _repo.GetBookByIdAsync(bookId);

            if (book == null)
            {
                _logger.LogInformation("book for edit not found by id: {Id}", bookId);
                return ServiceResult<BookModifyViewModel>.Fail("Книга не найдена");
            }

            _logger.LogInformation("found book by id ({Id}): {@Book}", bookId, book);

            var viewModel = new BookModifyViewModel
            {
                Authors = authors,
                Book = book,
            };

            return ServiceResult<BookModifyViewModel>.Ok(viewModel, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get book modify view model");
            return ServiceResult<BookModifyViewModel>.Fail(ServiceResult.SomethingWentWrong);
        }
    }


    public async Task<ServiceResult> UpdateAsync(int id, Book book)
    {
        try
        {
            var bookToUpdate = await _repo.GetBookByIdAsync(id);
            if (bookToUpdate == null)
            {
                _logger.LogInformation("book not found by id: {Id}", id);
                return ServiceResult.Fail("Книга не найдена");
            }

            _logger.LogInformation("book for update by id ({Id}) found: {@Book}", id, bookToUpdate);

            bookToUpdate.Title = book.Title;
            bookToUpdate.PageCount = book.PageCount;
            bookToUpdate.AuthorId = book.AuthorId;

            await _repo.UpdateAsync(bookToUpdate);
            return ServiceResult.Ok("Данные книги обновлены");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to update book ({Id})", id);
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        try
        {
            var book = await _repo.GetBookByIdAsync(id);
            _logger.LogInformation("founded book for delete: {@Book}", book);

            if (book == null)
            {
                _logger.LogInformation("attempt to get not existed book by id for delete: {Id}", id);
                return ServiceResult.Fail("Книга не найдена");
            }

            await _repo.DeleteAsync(book);
            return ServiceResult.Ok("Книга удалена");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to delete book ({Id})", id);
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }
}
