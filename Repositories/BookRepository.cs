using lib.Models;
using lib.Data;
using Microsoft.EntityFrameworkCore;

namespace lib.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibContext _context;

    public BookRepository(LibContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetBooksAsync()
    {
        return await _context.Books
          .Include(b => b.Author)
          .ToListAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task StoreAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book book)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}
