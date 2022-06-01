using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.DDD.Favorite
{
    [Table("favorites")]
    public record Favorite : BaseModel
    {
        public Favorite(Guid userId, Guid bookId)
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

        public virtual Book.Book Book { get; set; } = default!;

        public virtual User.User User { get; set; } = default!;
    }
}