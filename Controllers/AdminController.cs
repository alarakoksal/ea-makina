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
            if (username == _configuration["Admin:Username"] && password == _configuration["Admin:Password"])
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
            // DIŞARIYA BAĞLANMA YOK, DOĞRUDAN VERİTABANINDAN ÇEKİYOR
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        [Route("urun-ekle")]
        public IActionResult AddProduct() => View(new Product());

        [HttpPost]
        [Route("urun-ekle")]
        public IActionResult AddProduct(Product product, IFormFile ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var imagesPath = Path.Combine(wwwrootPath, "images");

                if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);

                var filePath = Path.Combine(imagesPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                product.ImageUrl = "/images/" + fileName;
            }

            // BURASI KRİTİK: HttpClient yerine doğrudan DB'ye ekliyoruz
            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Panel");
        }

        [HttpGet]
        [Route("urun-sil/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
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
            _context.Products.Update(product);
            _context.SaveChanges();
            return RedirectToAction("Panel");
        }
    }
}