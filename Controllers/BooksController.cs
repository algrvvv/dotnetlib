using lib.Data;
using lib.Models;
using lib.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lib.Controllers;

public class BooksController : Controller
{
    private readonly ILogger<BooksController> _logger;
    private readonly IBookService _service;
    private readonly LibContext _context;

    public BooksController(ILogger<BooksController> logger, LibContext context, IBookService service)
    {
        _logger = logger;
        _service = service;
        _context = context;
    }

    /// <summary>
    /// получение всех книг и их вывод
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var res = await _service.GetBooksAsync();
        if (!res.IsSuccess)
        {
            TempData[res.Status] = res.Message;
            return RedirectToAction("Index", "Home");
        }

        return View(res.Data);
    }

    public async Task<IActionResult> Show(int id)
    {
        var res = await _service.GetBookByIdAsync(id);
        if (!res.IsSuccess)
        {
            TempData[res.Status] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    public async Task<IActionResult> Create()
    {
        var res = await _service.GetModifyVM();
        if (!res.IsSuccess)
        {
            TempData[res.Status] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookModifyViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Получены некорректные данные";
            return RedirectToAction("Index");
        }

        var res = await _service.StoreAsync(viewModel.Book);
        if (!res.IsSuccess)
        {
            TempData[res.Status] = res.Message;
            return View(viewModel);
        }

        TempData[res.Status] = res.Message;
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _service.GetModifyVM(id);
        if (!res.IsSuccess)
        {
            TempData[res.Status] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookModifyViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Получены некорректные данные";
            return RedirectToAction("Index");
        }

        var res = await _service.UpdateAsync(id, viewModel.Book);
        if (!res.IsSuccess)
        {
            TempData[res.Status] = res.Message;
            return RedirectToAction("Index");
        }

        TempData[res.Status] = res.Message;
        return RedirectToAction("Edit");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await _service.DeleteAsync(id);
        TempData[res.Status] = res.Message;
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
