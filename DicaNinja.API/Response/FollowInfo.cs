
using DicaNinja.API.Enums;

namespace DicaNinja.API.Response;

public record FollowInfo(Guid FollowerId, EnumStatusFollow Status);
