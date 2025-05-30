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

    public async Task<IActionResult> Create()
    {
        try
        {
            var authors = await _context.Authors.ToListAsync();
            _logger.LogInformation("got authors for create new book. count: {Count}", authors.Count);


            var createViewModel = new BookModifyViewModel
            {
                Authors = authors
            };
            return View(createViewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get authors for create new book");
            TempData["Error"] = "Что то пошло не так";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookModifyViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("model state is invalid");
            TempData["Error"] = "Получены некорректные данные";
            return RedirectToAction("Index");
        }

        try
        {
            _context.Books.Add(viewModel.Book);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Книга добавлена";
            _logger.LogInformation("new book {@Book} saved successfully", viewModel.Book);

            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to store new book");
            ModelState.AddModelError(string.Empty, "Что то пошло не так");
            return View(viewModel);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            // получаем доступных список авторов для привязки к книге
            var authors = await _context.Authors.ToListAsync();
            _logger.LogInformation("got authors for edit book. count: {Count}", authors.Count);

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                _logger.LogInformation("book for edit not found by id: {Id}", id);
                TempData["Error"] = "Книга не найдена";
                return RedirectToAction("Index");
            }

            _logger.LogInformation("found book by id ({Id}): {@Book}", id, book);
            var viewModel = new BookModifyViewModel
            {
                Authors = authors,
                Book = book,
            };

            return View(viewModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get book or authors for update book by id: {Id}", id);
            TempData["Error"] = "Что то пошло не так";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookModifyViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("got invalid model state for edit book");
            TempData["Error"] = "Получены некорректные данные";
            return RedirectToAction("Index");
        }

        try
        {
            var bookToUpdate = await _context.Books.FindAsync(id);
            if (bookToUpdate == null)
            {
                _logger.LogInformation("book not found by id: {Id}", id);
                TempData["Error"] = "Книга для редактирования не найдена";
                return RedirectToAction("Index");
            }

            _logger.LogInformation("book for update by id ({Id}) found: {@Book}", id, bookToUpdate);

            bookToUpdate.Title = viewModel.Book.Title;
            bookToUpdate.PageCount = viewModel.Book.PageCount;
            bookToUpdate.AuthorId = viewModel.Book.AuthorId;

            _context.Update(bookToUpdate);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Данные книги обновлены";
            _logger.LogInformation("book updated successfully: {@Book}", viewModel.Book);
            return RedirectToAction("Edit");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to edit book");
            TempData["Error"] = "Что то пошло не так";
            return RedirectToAction("Index");
        }
    }

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
