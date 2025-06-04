using lib.Models;

namespace lib.Services;

public interface IPublisherService
{
    Task<ServiceResult<PublisherAuthorViewModel>> GetAuthorViewModelAsync();
    Task<ServiceResult<PublisherAuthorViewModel>> GetAuthorViewModelAsync(int id);
    Task<ServiceResult<Publisher>> GetPublisherByIdAsync(int id);
    Task<ServiceResult<List<Publisher>>> GetPublishersAsync();
    Task<ServiceResult> CreateAsync(Publisher publisher, List<int> authorIds);
    Task<ServiceResult> EditAsync(int id, PublisherAuthorViewModel vm);
    Task<ServiceResult> DeleteAsync(int id);
}
