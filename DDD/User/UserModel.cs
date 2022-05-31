using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using BookSearch.API.Abstracts;
using BookSearch.API.DDD.Person;
using BookSearch.API.DDD.RefreshToken;

namespace BookSearch.API.DDD.User;

[Table("users")]
public record UserModel : BaseModel
{
    public UserModel()
    {
        RefreshTokens = new List<RefreshTokenModel>();
    }

    public UserModel(Guid id, string username, string email, PersonModel personModel) : this(id, username, email)
    {
        PersonModel = personModel;
    }

    public UserModel(Guid id, string username, string email) : this()
    {
        Id = id;
        Username = username;
        Email = email;
    }

    public UserModel(string username, string password, string email, PersonModel personModel) : this()
    {
        Username = username;
        Password = password;
        Email = email;
        PersonModel = personModel;
    }

    public UserModel(Guid id, string username, string email, string password) : this(id, username, email)
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
    public PersonModel PersonModel { get; set; } = default!;

    [JsonIgnore]
    public List<RefreshTokenModel> RefreshTokens { get; set; }

}