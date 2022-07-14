
using DicaNinja.API.Enums;

namespace DicaNinja.API.Response;

public class FollowInfo
{
    public Guid FollowerId { get; init; }
    public EnumStatusFollow Status { get; init; }

    public FollowInfo(Guid followerId, EnumStatusFollow status)
    {
        FollowerId = followerId;
        Status = status;
    }
}
