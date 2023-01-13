
using DicaNinja.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicaNinja.API.Models;

[Table("books")]
public class Book : BaseModel
{
    public Book()
    {

    }

    public Book(string Title, string Subtitle, string Language, string Description, int PageCount, string Publisher, string PublicationDate, string Image, double AverageRating)
    {

        this.Title = Title;
        this.Subtitle = Subtitle;
        this.Language = Language;
        this.Description = Description;
        this.PageCount = PageCount;
        this.Publisher = Publisher;
        this.PublicationDate = PublicationDate;
        this.Image = Image;
        this.AverageRating = AverageRating;
    }

    [Column("title")]
    [Required]
    public string Title { get; private set; } = string.Empty;

    [Column("subtitle")]
    public string? Subtitle { get; set; }

    [Column("language")]
    public string? Language { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("page_count")]
    public int? PageCount { get; set; }

    [Column("publisher")]
    public string? Publisher { get; set; }

    [Column("publication_date")]
    public string? PublicationDate { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("average_ratting")]
    public double? AverageRating { get; set; }

    [Column("preview_link")]
    public string? PreviewLink { get; set; }

    public virtual List<Identifier> Identifiers { get; private set; } = new();

    public virtual List<Author> Authors { get; private set; } = new();

    public virtual List<Category> Categories { get; private set; } = new();

    public virtual List<Bookmark> Bookmarks { get; private set; } = new();

    public virtual List<Review> Reviews { get; private set; } = new();

    public virtual List<Hint> Hints { get; private set; } = new();

}
