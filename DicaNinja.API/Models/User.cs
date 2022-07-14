
using DicaNinja.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;

namespace DicaNinja.API.Models;

[Table("users")]
public class User : BaseModel
{
    public User()
    {
    }

    public User(Guid id, string username, string email)
    {
        Id = id;
        Username = username;
        Email = email;
    }

    public User(Guid id, string username, string email, string password) : this(id, username, email)
    {
        Password = password;
    }

    public User(string username, string firstname, string lastname, string email, string password)
    {
        Username = username;
        FirstName = firstname;
        LastName = lastname;
        Email = email;
        Password = password;
    }

    public User(Guid id, string username, string email, string firstName, string lastName, string? image)
    {
        Id = id;
        Username = username;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Image = image;
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

    [Required, MinLength(2), MaxLength(48)]
    [Column("first_name")]
    public string FirstName { get; set; } = default!;

    [Required, MinLength(2), MaxLength(48)]
    [Column("last_name")]
    public string LastName { get; set; } = default!;

    [Column("image")]
    public string? Image { get; set; }

    [Column("description")]
    [MaxLength(200)]
    [MinLength(5)]
    public string? Description { get; set; }

    [JsonIgnore]
    public virtual List<RefreshToken> RefreshTokens { get; private set; } = new();

    [JsonIgnore]
    public virtual List<Bookmark> Bookmarks { get; private set; } = new();

    public virtual List<Review> Reviews { get; private set; } = new();

    public virtual List<Follower> Following { get; private set; } = new();

    public virtual List<Follower> Followers { get; private set; } = new();

    public void SetPassword(string password)
    {
        Password = password;
    }
}
