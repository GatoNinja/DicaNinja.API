using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.Models;

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
    public string Name { get; set; }

    public virtual IEnumerable<Book> Books { get; set; } = default!;
}