using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.Models;

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

    public virtual List<Book> Books { get; set; } = default!;
}
