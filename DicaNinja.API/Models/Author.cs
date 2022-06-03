
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
        this.Name = name;
    }

    [Column("name")]
    [Required]
    public string Name { get; set; } = default!;

    public virtual IEnumerable<Book> Books { get; set; } = default!;
}
