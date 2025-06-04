using lib.Models;

namespace lib.Repositories;

public interface IPublisherRepository
{
    Task<List<Publisher>> GetPublishersAsync();
    Task<Publisher?> GetPublisherByIdAsync(int id);
    Task<Publisher?> GetPblByIdWithIncsAsync(int id);
    Task DeleteAsync(Publisher publisher);
}
