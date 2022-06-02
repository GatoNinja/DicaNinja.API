using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.Models;

[Table("bookmarks")]
public record Bookmark : BaseModel
{
    public Bookmark(Guid userId, Guid bookId)
    {
        UserId = userId;
        BookId = bookId;
    }

    [Column("user_id")]
    [Required]
    public Guid UserId { get; set; }

    [Column("book_id")]
    [Required]
    public Guid BookId { get; set; }

    public virtual Book Book { get; set; } = default!;

    public virtual User User { get; set; } = default!;
}
