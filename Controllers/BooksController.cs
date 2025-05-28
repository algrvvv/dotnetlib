using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using lib.Models;

namespace lib.Controllers;

public class BooksController : Controller
{
    private readonly ILogger<BooksController> _logger;

    public BooksController(ILogger<BooksController> logger)
    {
        _logger = logger;
    }

    public IActionResult Show()
    {
        return View();
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
