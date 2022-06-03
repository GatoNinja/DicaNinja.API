using BookSearch.API.Enums;

namespace BookSearch.API.Response;

public record FollowInfo(Guid FollowerId, EnumStatusFollow Status);
