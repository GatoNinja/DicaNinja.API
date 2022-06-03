
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
        this.IsActive = true;
    }

    public RefreshToken(Guid id, string value, DateTimeOffset refreshTokenExpiryTime, User user)
    {
        this.Id = id;
        this.Value = value;
        this.RefreshTokenExpiryTime = refreshTokenExpiryTime;
        this.UserId = user.Id;
        this.User = user;
    }

    public RefreshToken(string refreshTokenValue, Guid userId, bool isActive)
    {
        this.Value = refreshTokenValue;
        this.IsActive = isActive;
        this.UserId = userId;
        this.IsActive = isActive;
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
    public User? User { get; set; }
}
