using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.Models;

[Table("categories")]
public class Category : BaseModel
{
    public Category()
    {

    }

    public Category(string categoryName) : this()
    {
        this.Name = categoryName;
    }

    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("book_id")]
    public Guid BookId { get; set; }

    public virtual IEnumerable<Book> Books { get; set; } = default!;
}
