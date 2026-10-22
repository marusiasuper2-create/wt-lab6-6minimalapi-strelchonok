using wt_lab6_6minimalapi_strelchonok.Data;
using wt_lab6_6minimalapi_strelchonok.Models;


var builder = WebApplication.CreateBuilder(args);

// Эти две строки нужны для Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

//3.1 + 3.3 список с фильтрацией
app.MapGet("/api/books", (string? search) =>
{
    var books = BookStore.Books.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        books = books.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    return Results.Ok(books);
});

//3.2 поиск по ID
app.MapGet("/api/books/{id}", (int id) =>
{
    var book = BookStore.Books.FirstOrDefault(b => b.Id == id);
    if (book == null) return Results.NotFound();
    return Results.Ok(book);
});

// POST: /api/books (вариант для 3 баллов)
app.MapPost("/api/books", (Book book) =>
{
    //ID
    book.Id = BookStore.Books.Count == 0 ? 1 : BookStore.Books.Max(b => b.Id) + 1;
    BookStore.Books.Add(book);

    //201 и ссылка на новую книгу
    return Results.Created($"/api/books/{book.Id}", book);
});

//5.1 PUT
app.MapPut("/api/books/{id}", (int id, Book updatedBook) =>
{
    var book = BookStore.Books.FirstOrDefault(b => b.Id == id);
    if (book == null) return Results.NotFound();

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    book.Year = updatedBook.Year;
    book.Price = updatedBook.Price;
    book.IsAvailable = updatedBook.IsAvailable;

    return Results.Ok(book);
});

//5.2 DELETE
app.MapDelete("/api/books/{id}", (int id) =>
{
    var book = BookStore.Books.FirstOrDefault(b => b.Id == id);
    if (book == null) return Results.NotFound();

    BookStore.Books.Remove(book);
    return Results.NoContent();
});
app.Run();