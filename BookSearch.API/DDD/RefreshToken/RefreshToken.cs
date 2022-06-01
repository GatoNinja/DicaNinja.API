using BookSearch.API.Abstracts;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookSearch.API.DDD.RefreshToken
{
    [Table("refresh_tokens")]
    public sealed record RefreshToken : BaseModel
    {
        public RefreshToken()
        {
            IsActive = true;
        }

        public RefreshToken(Guid id, string value, DateTimeOffset refreshTokenExpiryTime, User.User userModel)
        {
            Id = id;
            Value = value;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
            UserId = userModel.Id;
            UserModel = userModel;
        }

        public RefreshToken(string refreshTokenValue, Guid userId, bool isActive)
        {
            Value = refreshTokenValue;
            IsActive = isActive;
            UserId = userId;
            IsActive = isActive;
        }

        [Required, MaxLength(255), MinLength(32)]
        [Column("value")]
        public string Value { get; set; } = default!;

        [Required]
        [Column("refresh_token_expiry_time")]
        public DateTimeOffset RefreshTokenExpiryTime { get; set; }

        [Required, DefaultValue(true)]
        [Column("is_active")]
        public bool IsActive { get; set; }

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User.User? UserModel { get; set; }
    }
}