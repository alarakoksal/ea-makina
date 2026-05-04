using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ea_makina.Models;
using ea_makina.Data;

namespace ea_makina.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
    _logger = logger;
    _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
{
    return View();
}

   public IActionResult Kiralama()
{
    return View();
}

public IActionResult Revizyon()
{
    return View();
}

public IActionResult Yedek()
{
    var products = _context.Products.ToList();
    return View(products);
}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
