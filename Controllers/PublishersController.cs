using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lib.Models;
using lib.Data;

namespace lib.Controllers;

public class PublishersController : Controller
{
    private readonly ILogger<PublishersController> _logger;
    private readonly LibContext _context;

    public PublishersController(ILogger<PublishersController> logger, LibContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
}
