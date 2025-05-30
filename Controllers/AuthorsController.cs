using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lib.Models;
using lib.Data;

namespace lib.Controllers;

public class AuthorsController : Controller
{
    private readonly ILogger<AuthorsController> _logger;
    private readonly LibContext _context;

    public AuthorsController(ILogger<AuthorsController> logger, LibContext context)
    {
        _logger = logger;
        _context = context;
    }

    // <summary>
    // вывод списка всех авторов
    // </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var authors = await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();

            return View(authors);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get author list");
            return RedirectToAction("Index", "Home");
        }
    }

    /// <summary>
    /// отображение подробной информации об авторе
    /// </summary>
    /// <param name="id">айди автора для просмотра</param>
    public async Task<IActionResult> Show(int id)
    {
        try
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            _logger.LogInformation("got author for show more info: {@Author}", author);
            return View(author);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get data for show author info");
            TempData["Error"] = "Что то пошло не так";
            return RedirectToAction("Index");
        }
    }

    // <summary>
    //  открыть страницу для создания нового автора
    // </summary>
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("FullName, Desc")] Author author)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("invalid model state while creating new author {@Author}", author);
            ModelState.AddModelError(string.Empty, "Некорректные данные");
            return View(author);
        }

        try
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            _logger.LogInformation("author {@Author} created successfully", author);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to create new author");
            ModelState.AddModelError(string.Empty, "Что то пошло не так");
            return View(author);
        }

        return RedirectToAction("Index");
    }

    // <summary>
    //  открыть страницу для редактирования
    // </summary>
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var author = await _context.Authors.Where(a => a.Id == id).FirstAsync();
            _logger.LogInformation("got author for edit: {@Author}", author);
            return View(author);
        }
        catch (Exception e)
        {
            // TODO: добавить обработку ошибки с точки зрения клиента (сообщение и тд)
            _logger.LogError("failed to get author info for edit: {e}", e);
            return Redirect("Error");
        }
    }

    /// <summary>
    ///  обработка запроса на редактирование информации об авторе
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id, FullName, Desc")] Author author)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("got invalid author model while edit: {@Author}", author);
            ModelState.AddModelError(string.Empty, "Некорректные данные");
            return View(author);
        }

        try
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
            _logger.LogInformation("author {@Author} updated successfully", author);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to update author: {@Author}", author);
            ModelState.AddModelError(string.Empty, "Что то пошло не так");
            return View(author);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            _logger.LogInformation("got author for delete by id: {@Author}", author);
            return View(author);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to get author for delete by id: {Id}", id);
            ModelState.AddModelError(string.Empty, "Что то пошло не так");
            return RedirectToAction("Index");
        }
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var author = await _context.Authors.FindAsync(id);
            _logger.LogInformation("got author for delete action by id ({Id}): {@Author}", id, author);

            if (author == null)
            {
                _logger.LogInformation("attempt to get not existed author by id: {Id}", id);
                TempData["Error"] = "Автор не найден";
                return RedirectToAction("Index");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            _logger.LogInformation("author by id ({Id}) deleted successfully", id);
            TempData["Success"] = "Автор удален";
            return RedirectToAction("Index");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to delete author by id: {Id}", id);
            TempData["Error"] = "Что то пошло не так";
            return RedirectToAction("Delete");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
