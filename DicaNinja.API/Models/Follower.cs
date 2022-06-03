using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DicaNinja.API.Abstracts;

namespace DicaNinja.API.Models;

[Table("followers")]
public class Follower : BaseModel
{
    public Follower()
    {

    }

    public Follower(Guid userId, Guid followedUserId)
    {
        this.UserId = userId;
        this.FollowedId = followedUserId;
    }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("follower_id")]
    public Guid FollowedId { get; set; }

    public virtual User User { get; set; } = default!;
    public virtual User FollowedUser { get; set; } = default!;
}
