namespace BookSearch.API.DDD.RefreshToken
{
    public interface IRefreshTokenRepository
    {
        string GenerateRefreshToken();

        Task<RefreshToken?> GetRefreshTokenAsync(string username, string value);

        Task SaveRefreshTokenAsync(Guid userId, string refreshToken);

        Task InvalidateAsync(string value);
    }
}