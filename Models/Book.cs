namespace wt_lab7_7postgres_efcore_strelchonok.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }
}