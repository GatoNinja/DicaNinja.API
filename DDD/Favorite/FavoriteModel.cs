using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.DDD.Favorite;

[Table("favorites")]
public record FavoriteModel: BaseModel
{
    public FavoriteModel(Guid userId, string identifier, string type)
    {
        UserId = userId;
        Identifier = identifier;
        Type = type;
    }

    [Column("user_id")]
    [Required]
    public Guid UserId { get; set; }

    [Column("identifier")]
    [Required]
    public string Identifier { get; set; } = default!;

    [Column("type")]
    [Required]
    public string Type { get; set; } = default!;
}
