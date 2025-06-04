using Microsoft.AspNetCore.Mvc;
using lib.Models;
using lib.Data;
using lib.Services;

namespace lib.Controllers;

public class PublishersController : Controller
{
    private readonly LibContext _context;
    private readonly IPublisherService _service;

    public PublishersController(LibContext context, IPublisherService service)
    {
        _context = context;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var res = await _service.GetPublishersAsync();
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index", "Home");
        }

        return View(res.Data);
    }

    public async Task<IActionResult> Show(int id)
    {
        var res = await _service.GetPublisherByIdAsync(id);
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    public async Task<IActionResult> Create()
    {
        var ap = await _service.GetAuthorViewModelAsync();
        if (!ap.IsSuccess)
        {
            TempData["Error"] = ap.Message;
            return RedirectToAction("Index");
        }

        return View(ap.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PublisherAuthorViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Некорректные данные";
            return View(viewModel);
        }

        var res = await _service.CreateAsync(viewModel.Publisher, viewModel.SelectedAuthors);
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index");
        }

        if (res.Message != null) TempData["Success"] = res.Message;
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var res = await _service.GetAuthorViewModelAsync(id);
        if (!res.IsSuccess)
        {
            TempData[res.Status] = res.Message;
            return RedirectToAction("Index");
        }

        return View(res.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PublisherAuthorViewModel vm)
    {
        var res = await _service.EditAsync(id, vm);
        TempData[res.Status] = res.Message;
        return RedirectToAction("Edit");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await _service.DeleteAsync(id);
        if (!res.IsSuccess)
        {
            TempData["Error"] = res.Message;
            return RedirectToAction("Index");
        }

        if (res.Message != null) TempData["Success"] = res.Message;
        return RedirectToAction("Index");
    }
}
