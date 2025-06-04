using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lib.Services;
using lib.Models;
using lib.Data;

namespace lib.Controllers;

public class AuthorsController : Controller
{
    private readonly IAuthorService _service;
    private readonly LibContext _context;

    public AuthorsController(IAuthorService service, LibContext context)
    {
        _service = service;
        _context = context;
    }

    /// <summary>
    /// вывод списка всех авторов
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var res = await _service.GetAuthorsAsync();
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index", "Home");
        }

        return View(res.Data);
    }

    /// <summary>
    /// отображение подробной информации об авторе
    /// </summary>
    /// <param name="id">айди автора для просмотра</param>
    public async Task<IActionResult> Show(int id)
    {
        var res = await _service.GetAuthorByIdAsync(id);
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    /// <summary>
    ///  открыть страницу для создания нового автора
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("FullName, Desc")] Author author)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Получены некорректные данные";
            return RedirectToAction("Index");
        }

        var res = await _service.StoreAsync(author);
        if (!res.IsSuccess) TempData["Error"] = res.Message;
        else TempData["Success"] = res.Message;

        return RedirectToAction("Index");
    }

    /// <summary>
    /// отдаем страницу для редактирования
    /// </summary>
    public async Task<IActionResult> Edit(int id)
    {
        var res = await _service.GetAuthorByIdAsync(id);
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    /// <summary>
    ///  обработка запроса на редактирование информации об авторе
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("Id, FullName, Desc")] Author author)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Получены некорректные данные";
            return RedirectToAction("Index");
        }

        var res = await _service.UpdateAsync(author);
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index");
        }

        TempData["Success"] = res.Message;
        return RedirectToAction("Edit");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var res = await _service.GetAuthorByIdAsync(id);
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var res = await _service.DeleteByIdAsync(id);
        TempData[res.Status] = res.Message;
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
