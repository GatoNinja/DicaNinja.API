using System.ComponentModel.DataAnnotations.Schema;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Enums;

namespace DicaNinja.API.Models;

[Table("hints")]
public class Hint: BaseModel
{
    public Hint()
    {

    }

    public Hint(Guid userId, Guid bookId, EnumHintStatus hintStatus): this()
    {
        UserId = userId;
        BookId = bookId;
        Liked = hintStatus == EnumHintStatus.Accepted;
    }

    public Guid UserId { get; set; }

    public Guid BookId { get; set; }

    public bool Liked { get; set; }

    public virtual User User { get; set; } = default!;

    public virtual Book Book { get; set; } = default!;
}
