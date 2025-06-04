using Microsoft.EntityFrameworkCore;
using lib.Models;
using lib.Repositories;
using lib.Data;

namespace lib.Services;

public class PublisherService : IPublisherService
{
    private readonly ILogger<PublisherService> _logger;
    private readonly IPublisherRepository _repo;
    private readonly IAuthorRepository _authorRepo;
    private readonly LibContext _context;

    public PublisherService(ILogger<PublisherService> logger, IPublisherRepository repo, IAuthorRepository authorRepo, LibContext context)
    {
        _logger = logger;
        _repo = repo;
        _authorRepo = authorRepo;
        _context = context;
    }

    public async Task<ServiceResult<List<Publisher>>> GetPublishersAsync()
    {
        try
        {
            var publishers = await _repo.GetPublishersAsync();
            return ServiceResult<List<Publisher>>.Ok(publishers, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get publishers for index");
            return ServiceResult<List<Publisher>>.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult<PublisherAuthorViewModel>> GetAuthorViewModelAsync()
    {
        try
        {
            var authors = await _authorRepo.GetAuthorsAsync();
            var publisherAuthor = new PublisherAuthorViewModel
            {
                AuthorsList = authors,
            };

            return ServiceResult<PublisherAuthorViewModel>.Ok(publisherAuthor, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get authors for create view model");
            return ServiceResult<PublisherAuthorViewModel>.Fail("");
        }
    }

    public async Task<ServiceResult<PublisherAuthorViewModel>> GetAuthorViewModelAsync(int publisherId)
    {
        try
        {
            var authors = await _authorRepo.GetAuthorsAsync();
            var publisher = await _repo.GetPblByIdWithIncsAsync(publisherId);
            if (publisher == null)
            {
                _logger.LogInformation("publisher not found by id: {Id}", publisherId);
                return ServiceResult<PublisherAuthorViewModel>.Fail("Издательство не найдено");
            }

            List<int> selectedAuthors = new();
            foreach (var ap in publisher.AuthorPublishers)
            {
                selectedAuthors.Add(ap.AuthorId);
            }

            var publisherAuthor = new PublisherAuthorViewModel
            {
                AuthorsList = authors,
                SelectedAuthors = selectedAuthors,
                Publisher = publisher,
            };

            return ServiceResult<PublisherAuthorViewModel>.Ok(publisherAuthor, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get authors for create view model");
            return ServiceResult<PublisherAuthorViewModel>.Fail("");
        }
    }

    public async Task<ServiceResult> CreateAsync(Publisher publisher, List<int> authorIds)
    {
        // начинаем транзацкцию
        using var tr = await _context.Database.BeginTransactionAsync();

        try
        {
            _logger.LogInformation("start transaction for create new publisher with authors");
            _logger.LogInformation("selected authors: {@SelectedAuthors}", authorIds);

            // добавляем издательство
            await _context.Publishers.AddAsync(publisher);
            await _context.SaveChangesAsync();

            var auhtorPublishers = authorIds.Select(authorId => new AuthorPublisher
            {
                AuthorId = authorId,
                PublisherId = publisher.Id,
            }).ToList();
            _logger.LogInformation("links list: {@AuthorPublishers}", auhtorPublishers);

            // добавляем привязки к авторам
            await _context.AuthorPublishers.AddRangeAsync(auhtorPublishers);
            await _context.SaveChangesAsync();

            // коммитим транзацкцию
            await tr.CommitAsync();
            return ServiceResult.Ok("Издательство успешно сохранено");
        }
        catch (Exception e)
        {
            // откатываем транзакцию
            await tr.RollbackAsync();
            _logger.LogError(e, "failed to create new publisher. rollback transaction...");
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        try
        {
            var publisherForDelete = await _repo.GetPblByIdWithIncsAsync(id);
            if (publisherForDelete == null) return ServiceResult.Fail("Издательство не найдено");

            await _repo.DeleteAsync(publisherForDelete);
            return ServiceResult.Ok("Издательство успешно удалено");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to delete publisher");
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult<Publisher>> GetPublisherByIdAsync(int id)
    {
        try
        {
            var publisher = await _repo.GetPblByIdWithIncsAsync(id);
            return ServiceResult<Publisher>.Ok(publisher, null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get publisher by id");
            return ServiceResult<Publisher>.Fail(ServiceResult.SomethingWentWrong);
        }
    }

    public async Task<ServiceResult> EditAsync(int id, PublisherAuthorViewModel vm)
    {
        await using var tr = await _context.Database.BeginTransactionAsync();

        try
        {
            var publisher = await _repo.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return ServiceResult.Fail("Издательство не найдено");
            }

            _logger.LogInformation("got publisher for edit: {@Publisher}", publisher);
            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();

            var existedLinks = await _context.AuthorPublishers
              .Where(ap => ap.PublisherId == id)
              .ToListAsync();

            _context.AuthorPublishers.RemoveRange(existedLinks);
            await _context.SaveChangesAsync();

            var newLinks = vm.SelectedAuthors.Select(authorId => new AuthorPublisher
            {
                AuthorId = authorId,
                PublisherId = id,
            }).ToList();

            await _context.AuthorPublishers.AddRangeAsync(newLinks);
            await _context.SaveChangesAsync();

            await tr.CommitAsync();
            return ServiceResult.Ok("Издательство успешно изменено");
        }
        catch (Exception e)
        {
            await tr.RollbackAsync();
            _logger.LogError(e, "failed to edit publisher by id: {Id}", id);
            return ServiceResult.Fail(ServiceResult.SomethingWentWrong);
        }
    }
}
