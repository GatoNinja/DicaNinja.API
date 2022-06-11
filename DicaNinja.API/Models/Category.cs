
using DicaNinja.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicaNinja.API.Models;

[Table("categories")]
public class Category : BaseModel
{
    public Category()
    {

    }

    public Category(string categoryName) : this()
    {
        Name = categoryName;
    }

    [Column("name")]
    [Required]
    public string Name { get; private set; } = string.Empty;

    [Column("book_id")]
    public Guid BookId { get; set; }

    public virtual IEnumerable<Book> Books { get; private set; } = default!;
}
