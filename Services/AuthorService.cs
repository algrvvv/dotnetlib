using lib.Repositories;
using lib.Models;

namespace lib.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repo;
    private readonly ILogger<AuthorService> _logger;

    public AuthorService(ILogger<AuthorService> logger, IAuthorRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<ServiceResult<List<Author>>> GetAuthorsAsync()
    {
        try
        {
            var authors = await _repo.GetAuthorsAsync();
            return ServiceResult<List<Author>>.Ok(authors, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get authors list");
            return ServiceResult<List<Author>>.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult<Author?>> GetAuthorByIdAsync(int id)
    {
        try
        {
            var author = await _repo.GetAuthorByIdAsync(id);
            if (author == null)
            {
                _logger.LogInformation("author by id not found: {Id}", id);
                return ServiceResult<Author?>.Fail("Автор не найден");
            }

            _logger.LogInformation("got author by id: {@Author}", author);
            return ServiceResult<Author?>.Ok(author, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get author by id");
            return ServiceResult<Author?>.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult> StoreAsync(Author author)
    {
        try
        {
            await _repo.StoreAsync(author);
            _logger.LogInformation("author {@Author} created successfully", author);
            return ServiceResult.Ok("Новый автор добавлен");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to save new author");
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult> UpdateAsync(Author author)
    {
        try
        {
            await _repo.UpdateAsync(author);
            _logger.LogInformation("author {@Author} updated successfully", author);
            return ServiceResult.Ok("Данные автора обновлены");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to update author data");
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult> DeleteByIdAsync(int id)
    {
        try
        {
            await _repo.DeleteAsync(id);
            return ServiceResult.Ok("Автор успешно удален");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to delete author by id: {Id}", id);
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }
}
