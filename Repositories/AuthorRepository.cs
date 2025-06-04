using lib.Models;
using lib.Data;
using Microsoft.EntityFrameworkCore;

namespace lib.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly LibContext _context;

    public AuthorRepository(LibContext context)
    {
        _context = context;
    }

    public async Task<List<Author>> GetAuthorsAsync()
    {
        return await _context.Authors
          .Include(a => a.Books)
          .Include(a => a.AuthorPublishers)
            .ThenInclude(ap => ap.Publisher)
          .ToListAsync();
    }

    public async Task<Author?> GetAuthorByIdAsync(int id)
    {
        return await _context.Authors
          .Where(a => a.Id == id)
          .Include(a => a.Books)
          .Include(a => a.AuthorPublishers)
            .ThenInclude(ap => ap.Publisher)
          .FirstOrDefaultAsync();
    }

    public async Task StoreAsync(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Author author)
    {
        _context.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            throw new Exception("author not found");
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}
