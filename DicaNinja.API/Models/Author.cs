
using DicaNinja.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicaNinja.API.Models;

[Table("authors")]
public class Author : BaseModel
{
    public Author()
    {

    }

    public Author(string name)
    {
        Name = name;
    }

    [Column("name")]
    [Required]
    public string Name { get; private set; } = default!;

    public virtual IEnumerable<Book> Books { get; private set; } = default!;
}
