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
        var authors = await _context.Authors.ToListAsync();
        return View(authors);
    }

    /// <summary>
    /// отображение подробной информации об авторе
    /// </summary>
    /// <param name="id">айди автора для просмотра</param>
    public IActionResult Show(int id)
    {
        return Content("asdfasdfasdfasdf: " + id);
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

        return RedirectToAction("Edit");
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        return Content("delete: " + id);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
