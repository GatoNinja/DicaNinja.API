using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.Models;

[Table("reviews")]
public class Review : BaseModel
{

    public Review()
    {

    }
    public Review(Guid userId, Guid bookId, string text, int rating)
    {
        this.UserId = userId;
        this.BookId = bookId;
        this.Text = text;
        this.Rating = rating;
    }

    [Column("text")]
    [MaxLength(2048)]
    public string Text { get; set; } = default!;

    [Column("rating")]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("book_id")]
    public Guid BookId { get; set; }
}
