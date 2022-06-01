namespace BookSearch.API.DDD.Favorite;

public record FavoriteDTO
(
    Guid Id,
    Guid UserId,
    string Identifier,
    string Type
);