using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.DDD.Author
{
    [Table("authors")]
    public record Author : BaseModel
    {
        public Author(string name)
        {
            Name = name;
        }

        [Column("name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual List<Book.Book> Books { get; set; } = default!;
    }
}