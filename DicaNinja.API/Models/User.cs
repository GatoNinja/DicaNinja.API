using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using DicaNinja.API.Abstracts;

namespace DicaNinja.API.Models;

[Table("users")]
public class User : BaseModel
{
    public User()
    {
    }

    public User(Guid id, string username, string email)
    {
        this.Id = id;
        this.Username = username;
        this.Email = email;
    }

    public User(Guid id, string username, Person person)
    {
        this.Id = id;
        this.Username = username;
        this.Person = person;
    }

    public User(string username, string password, string email, Person person)
    {
        this.Username = username;
        this.Password = password;
        this.Email = email;
        this.Person = person;
    }

    public User(Guid id, string username, string email, string password) : this(id, username, email)
    {
        this.Password = password;
    }

    public User(Guid id, string username, string email, Person person) : this(id, username, email)
    {
        this.Person = person;
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
    public virtual Person Person { get; set; } = default!;

    [JsonIgnore]
    public virtual List<RefreshToken> RefreshTokens { get; set; } = default!;

    [JsonIgnore]
    public virtual List<Bookmark> Bookmarks { get; set; } = default!;

    public virtual List<Review> Reviews { get; set; } = default!;

    public virtual List<Follower> Following { get; set; } = default!;

    public virtual List<Follower> Followers { get; set; } = default!;
}
