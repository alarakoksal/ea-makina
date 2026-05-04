using ea_makina.Data;
using ea_makina.Models; // 1. EKSİK OLAN BUYDU: Product sınıfını tanımasını sağlar
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı Ayarı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NOT: Loglarda hata veren 'AddDatabaseDeveloperPageExceptionFilter' satırını sildik.

var app = builder.Build();

// Veritabanını oluştur ve içine örnek veri ekle
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.Add(new Product
        {
            Name = "Test Ürün",
            Description = "Test açıklama",
            Price = 0,
            ImageUrl = ""
        });
        db.SaveChanges();
    }
}

// Geliştirme modu ayarları
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();