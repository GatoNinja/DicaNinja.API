using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.DDD.BookIdentifier
{
    [Table("book_identifiers")]
    public record BookIdentifier : BaseModel
    {
        public BookIdentifier(string isbn, string type)
        {
            Isbn = isbn;
            Type = type;
        }

        [Column("isbn")]
        [Required]
        public string Isbn { get; set; } = string.Empty;

        [Column("type")]
        [Required]
        public string Type { get; set; } = string.Empty;

        [Column("book_id")]
        public Guid BookId { get; set; }

        public virtual Book.Book Book { get; set; } = default!;
    }
}