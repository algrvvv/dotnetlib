using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lib.Models;
using lib.Data;
using Microsoft.EntityFrameworkCore;

namespace lib.Controllers;

public class BooksController : Controller
{
    private readonly ILogger<BooksController> _logger;
    private readonly LibContext _context;

    public BooksController(ILogger<BooksController> logger, LibContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// получение всех книг и их вывод
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .ToListAsync();

            _logger.LogInformation("got books for index page. books count: {Count}", books.Count);
            return View(books);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get books for index page");
            TempData["Error"] = "Что то пошло не так";
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Show(int id)
    {
        try
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            _logger.LogInformation("");
            return View(book);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get data for show book info");
            TempData["Error"] = "Что то пошло не так";
            return RedirectToAction("Index");
        }
    }

    }

    public IActionResult Edit(int id)
    {
        return Content("id: " + id);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
