using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookSearch.API.Abstracts;

public abstract class BaseModel
{
    public BaseModel()
    {
        this.Created = DateTimeOffset.Now;
        this.Updated = DateTimeOffset.Now;
        this.Id = Guid.Empty;
    }

    [Key, Required]
    [Column("id")]
    public Guid Id { get; set; }

    [JsonIgnore, Required]
    [Column("created")]
    public DateTimeOffset Created { get; set; }

    [Required, JsonIgnore]
    [Column("updated")]
    public DateTimeOffset Updated { get; set; }
}
