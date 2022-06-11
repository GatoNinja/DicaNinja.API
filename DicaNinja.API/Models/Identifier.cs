
using DicaNinja.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DicaNinja.API.Models;

[Table("identifiers")]
public class Identifier : BaseModel
{
    public Identifier()
    {

    }
    public Identifier(string isbn, string type)
    {
        Isbn = isbn;
        Type = type;
    }

    [Column("isbn")]
    [Required]
    public string Isbn { get; private set; } = string.Empty;

    [Column("type")]
    [Required]
    public string Type { get; private set; } = string.Empty;

    [Column("book_id")]
    public Guid BookId { get; set; }

    public virtual Book Book { get; private set; } = default!;
}
