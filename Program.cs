using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_strelchonok.Data;
using wt_lab7_7postgres_efcore_strelchonok.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    // Парсим URL, который выдает Render
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);

    connectionString = $"Host={uri.Host};" +
                       $"Port={uri.Port};" +
                       $"Database={uri.AbsolutePath.TrimStart('/')};" +
                       $"Username={Uri.UnescapeDataString(userInfo[0])};" +
                       $"Password={Uri.UnescapeDataString(userInfo[1])};" +
                       "SSL Mode=Disable";
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Автоматическое применение миграций при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Minimal API работает!");

app.MapGet("/api/info", () =>
{
    return new { Application = "Web Technologies", Technology = "Minimal API", Version = "1.0" };
});

//LINQ-запросы (ДОЛЖНЫ БЫТЬ ВЫШЕ чем /{id})
// Дороже 100
app.MapGet("/api/books/expensive", async (AppDbContext db) =>
    Results.Ok(await db.Books.Where(b => b.Price > 100).ToListAsync()));

//cорт по цене
app.MapGet("/api/books/sorted", async (AppDbContext db) =>
    Results.Ok(await db.Books.OrderBy(b => b.Price).ToListAsync()));

//доступные книги, сорт по названию
app.MapGet("/api/books/available", async (AppDbContext db) =>
    Results.Ok(await db.Books.Where(b => b.IsAvailable).OrderBy(b => b.Title).ToListAsync()));

//свой запрос (книги старше 1900 года)
app.MapGet("/api/books/classic", async (AppDbContext db) =>
    Results.Ok(await db.Books.Where(b => b.Year < 1900).ToListAsync()));


// 4. CRUD операции
// GET: Получение списка с фильтрацией
app.MapGet("/api/books", async (string? search, AppDbContext db) =>
{
    var query = db.Books.AsQueryable();

    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(b => b.Title.ToLower().Contains(search.ToLower()));
    }

    var books = await query.ToListAsync();
    return Results.Ok(books);
});

// GET: Поиск по ID
app.MapGet("/api/books/{id}", async (int id, AppDbContext db) =>
{
    var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
    if (book == null) return Results.NotFound();
    return Results.Ok(book);
});

// POST: Создание книги
app.MapPost("/api/books", async (Book book, AppDbContext db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/api/books/{book.Id}", book);
});

// PUT: Изменение книги
app.MapPut("/api/books/{id}", async (int id, Book updatedBook, AppDbContext db) =>
{
    var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
    if (book == null) return Results.NotFound();

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    book.Year = updatedBook.Year;
    book.Price = updatedBook.Price;
    book.IsAvailable = updatedBook.IsAvailable;

    await db.SaveChangesAsync();
    return Results.Ok(book);
});

// DELETE: Удаление книги
app.MapDelete("/api/books/{id}", async (int id, AppDbContext db) =>
{
    var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
    if (book == null) return Results.NotFound();

    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//POST добавление
app.MapPost("/api/books", async (Book book, AppDbContext db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/api/books/{book.Id}", book);
});

//PUT изменение
app.MapPut("/api/books/{id}", async (int id, Book updatedBook, AppDbContext db) =>
{
    var book = await db.Books.FirstOrDefaultAsync(x => x.Id == id);
    if (book == null) return Results.NotFound();

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    book.Year = updatedBook.Year;
    book.Price = updatedBook.Price;
    book.IsAvailable = updatedBook.IsAvailable;

    await db.SaveChangesAsync();
    return Results.Ok(book);
});

//DELETE удаление
app.MapDelete("/api/books/{id}", async (int id, AppDbContext db) =>
{
    var book = await db.Books.FirstOrDefaultAsync(x => x.Id == id);
    if (book == null) return Results.NotFound();

    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Запуск приложения (настройка порта для Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");