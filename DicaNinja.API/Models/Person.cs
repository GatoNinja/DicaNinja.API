
using DicaNinja.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DicaNinja.API.Models;

[Table("people")]
public class Person : BaseModel
{
    public Person()
    {

    }

    public Person(string firstname, string lastname)
    {
        FirstName = firstname;
        LastName = lastname;
    }

    [Required, MinLength(2), MaxLength(48)]
    [Column("first_name")]
    public string FirstName { get; set; } = default!;

    [Required, MinLength(2), MaxLength(48)]
    [Column("last_name")]
    public string LastName { get; set; } = default!;

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("image")]
    public string? Image { get; set; }

    [Column("description")]
    [MaxLength(200)]
    [MinLength(5)]
    public string? Description { get; set; }

    [JsonIgnore]
    public virtual User? User { get; set; }
}
