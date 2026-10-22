using wt_lab6_6minimalapi_strelchonok.Models;

namespace wt_lab6_6minimalapi_strelchonok.Data;

public static class BookStore
{
    public static List<Book> Books { get; } = new()
    {
        new Book { Id = 1, Title = "Преступление и наказание", Author = "Ф. М. Достоевский", Year = 1866, Price = 25.50m, IsAvailable = true },
        new Book { Id = 2, Title = "Война и мир", Author = "Л. Н. Толстой", Year = 1869, Price = 45.00m, IsAvailable = true },
        new Book { Id = 3, Title = "Мастер и Маргарита", Author = "М. А. Булгаков", Year = 1967, Price = 30.00m, IsAvailable = false },
        new Book { Id = 4, Title = "Герой нашего времени", Author = "М. Ю. Лермонтов", Year = 1846, Price = 15.00m, IsAvailable = true },
        new Book { Id = 5, Title = "Евгений Онегин", Author = "А. С. Пушкин", Year = 1833, Price = 20.00m, IsAvailable = false }
    };
}