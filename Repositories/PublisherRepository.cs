using lib.Data;
using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace lib.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly LibContext _context;
    private readonly ILogger<PublisherRepository> _logger;

    public PublisherRepository(ILogger<PublisherRepository> logger, LibContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<Publisher>> GetPublishersAsync()
    {
        return await _context.Publishers
            .Include(p => p.AuthorPublishers)
              .ThenInclude(ap => ap.Author)
            .ToListAsync();
    }

    public async Task<Publisher?> GetPblByIdWithIncsAsync(int id)
    {
        return await _context.Publishers
            .Include(p => p.AuthorPublishers)
              .ThenInclude(ap => ap.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Publisher?> GetPublisherByIdAsync(int id)
    {
        return await _context.Publishers.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task DeleteAsync(Publisher publisher)
    {
        _context.Publishers.Remove(publisher);
        await _context.SaveChangesAsync();
    }
}
