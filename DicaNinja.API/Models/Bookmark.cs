
using DicaNinja.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicaNinja.API.Models;

[Table("bookmarks")]
public class Bookmark : BaseModel
{
    public Bookmark()
    {

    }

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
