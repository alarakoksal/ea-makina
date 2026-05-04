using ea_makina.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Kaydı (Sadece bir tanesini tutun)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// 2. Veritabanı Seed (Başlangıç Verisi) İşlemi
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // SQLite kullanırken EnsureCreated yerine Migration kullanmanız daha sağlıklı olabilir
    // ama hızlı başlangıç için EnsureCreated çalışacaktır.
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

// 3. Middleware Sıralaması
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Development içindeyken bunu kullanmak daha doğrudur
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