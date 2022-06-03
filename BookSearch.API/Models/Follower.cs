using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookSearch.API.Abstracts;

namespace BookSearch.API.Models;

[Table("followers")]
public class Follower : BaseModel
{
    public Follower()
    {

    }

    public Follower(Guid userId, Guid followedUserId)
    {
        UserId = userId;
        FollowedId = followedUserId;
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
