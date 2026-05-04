using Microsoft.AspNetCore.Mvc;
using ea_makina.Data;
using ea_makina.Models;
using System.Linq;

namespace ea_makina.Controllers
{
    [Route("yonetim")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AdminController(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
{
    _context = context;
    _httpClient = httpClientFactory.CreateClient();
    _configuration = configuration;
}
        
        [HttpGet]
        [Route("")]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [Route("")]
        public IActionResult Login(string username, string password)
        {
           var adminUser = _configuration["Admin:Username"];
var adminPass = _configuration["Admin:Password"];

if (username == adminUser && password == adminPass)
            {
                return RedirectToAction("Panel");
            }

            ViewBag.Error = "Hatalı giriş!";
            return View();
        }

        
        [HttpGet]
        [Route("panel")]
        public IActionResult Panel()
{
    var products = _httpClient
        .GetFromJsonAsync<List<Product>>("https://ea-makina.onrender.com/api/products")
        .Result;

    return View(products);
}

        
        [HttpGet]
        [Route("urun-ekle")]
        public IActionResult AddProduct()
        {
            return View(new Product());
        }

        
        [HttpPost]
        [Route("urun-ekle")]
        public IActionResult AddProduct(Product product, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            var response = _httpClient.PostAsJsonAsync("http://localhost:5185/api/products", product).Result;

            return RedirectToAction("Panel");
        }

        [HttpGet]
[Route("urun-sil/{id}")]
public IActionResult Delete(int id)
{
    var response = _httpClient.DeleteAsync($"http://localhost:5185/api/products/{id}").Result;

    return RedirectToAction("Panel");
}

        
        [HttpGet]
        [Route("urun-duzenle/{id}")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            return View(product);
        }

        [HttpPost]
[Route("urun-duzenle/{id}")]
public IActionResult Edit(Product product)
{
    var response = _httpClient.PutAsJsonAsync(
        $"http://localhost:5185/api/products/{product.Id}",
        product
    ).Result;

    return RedirectToAction("Panel");
}
    }
}