
using BookSearch.API.Contexts.Configurations;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookSearch.API.Abstracts;

public abstract record BaseModel : ISoftDelete
{
    protected BaseModel()
    {
        Created = DateTimeOffset.Now;
        Updated = DateTimeOffset.Now;
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

    [JsonIgnore]
    [Column("deleted")]
    public DateTimeOffset? Deleted { get; set; }
}