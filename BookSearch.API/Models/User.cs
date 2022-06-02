using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using BookSearch.API.Abstracts;

namespace BookSearch.API.Models;

[Table("users")]
public record User : BaseModel
{
    public User()
    {
    }

    public User(Guid id, string username, string email) : this()
    {
        Id = id;
        Username = username;
        Email = email;
    }

    public User(string username, string password, string email, Person personModel) : this()
    {
        Username = username;
        Password = password;
        Email = email;
        PersonModel = personModel;
    }

    public User(Guid id, string username, string email, string password) : this(id, username, email)
    {
        Password = password;
    }

    [Required, MinLength(3), MaxLength(48)]
    [Column("username")]
    public string Username { get; set; } = default!;

    [Required, EmailAddress]
    [Column("email")]
    public string Email { get; set; } = default!;

    [Required, JsonIgnore]
    [Column("password")]
    public string Password { get; set; } = default!;

    [JsonIgnore]
    public virtual Person PersonModel { get; set; } = default!;

    [JsonIgnore]
    public virtual List<RefreshToken> RefreshTokens { get; set; } = default!;

    [JsonIgnore]
    public virtual List<Favorite> Favorites { get; set; } = default!;

}
