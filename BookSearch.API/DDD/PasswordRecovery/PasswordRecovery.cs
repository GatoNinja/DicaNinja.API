using BookSearch.API.Abstracts;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookSearch.API.DDD.PasswordRecovery
{
    [Table("password_recoveries")]
    public record PasswordRecovery : BaseModel
    {
        public PasswordRecovery()
        {
            IsActive = true;
            ExpireDate = DateTimeOffset.Now.AddHours(12);
        }

        public PasswordRecovery(User.User userModel) : this()
        {
            UserModel = userModel;
            UserId = userModel.Id;
        }

        [Required, DefaultValue(true)]
        [Column("is_active")]
        public bool IsActive { get; set; }

        [Required, MinLength(1), MaxLength(7)]
        [Column("code")]
        public string Code { get; set; } = default!;

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        [Column("user_id")]
        public User.User UserModel { get; set; } = default!;

        [Required]
        [Column("expire_date")]
        public DateTimeOffset ExpireDate { get; set; }
    }
}