
using BookSearch.API.Abstracts;
using BookSearch.API.DDD.User;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookSearch.API.DDD.Person;

[Table("people")]
public sealed record PersonModel : BaseModel
{
    public PersonModel()
    {

    }

    public PersonModel(string firstname, string lastname)
    {
        FirstName = firstname;
        Lastname = lastname;
    }

    [Required, MinLength(2), MaxLength(48)]
    [Column("first_name")]
    public string FirstName { get; set; } = default!;

    [Required, MinLength(2), MaxLength(48)]
    [Column("last_name")]
    public string Lastname { get; set; } = default!;

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [JsonIgnore]
    public UserModel? User { get; set; }
}