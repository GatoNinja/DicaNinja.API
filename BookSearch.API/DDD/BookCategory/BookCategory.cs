using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.DDD.BookCategory
{
    [Table("book_categories")]
    public record BookCategory : BaseModel
    {
        public BookCategory()
        {
            
        }

        public BookCategory(string categoryName) : this()
        {
            Name = categoryName;
        }

        [Column("name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column("book_id")]
        public Guid BookId { get; set; }

        public virtual Book.Book Book { get; set; } = default!;
    }
}