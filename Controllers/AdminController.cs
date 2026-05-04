using Microsoft.AspNetCore.Mvc;
using ea_makina.Data;
using ea_makina.Models;
using Microsoft.EntityFrameworkCore;

namespace ea_makina.Controllers
{
    [Route("yonetim")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Login() => View();

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
        public async Task<IActionResult> Panel()
        {
            // HttpClient yerine doğrudan veritabanından listeyi çekiyoruz
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        [Route("urun-ekle")]
        public IActionResult AddProduct() => View(new Product());

        [HttpPost]
        [Route("urun-ekle")]
        public async Task<IActionResult> AddProduct(Product product, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                product.ImageUrl = "/images/" + fileName;
            }

            // Doğrudan veritabanına ekle
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Panel");
        }

        [HttpGet]
        [Route("urun-sil/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Panel");
        }

        [HttpGet]
        [Route("urun-duzenle/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return View(product);
        }

        [HttpPost]
        [Route("urun-duzenle/{id}")]
        public async Task<IActionResult> Edit(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Panel");
        }
    }
}