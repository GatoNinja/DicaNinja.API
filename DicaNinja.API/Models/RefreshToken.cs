
using DicaNinja.API.Abstracts;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DicaNinja.API.Models;

[Table("refresh_tokens")]
public sealed class RefreshToken : BaseModel
{
    public RefreshToken()
    {
        IsActive = true;
    }

    public RefreshToken(Guid id, string value, DateTimeOffset refreshTokenExpiryTime, User user)
    {
        Id = id;
        Value = value;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
        UserId = user.Id;
        User = user;
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
    public string Value { get; private set; } = default!;

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
    public User? User { get; set; }
}
