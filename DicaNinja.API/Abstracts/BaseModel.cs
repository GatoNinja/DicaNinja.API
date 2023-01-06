using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DicaNinja.API.Abstracts;

public abstract class BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [JsonIgnore, Required]
    [Column("created")]
    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

    [JsonIgnore]
    [Column("updated")]
    public DateTimeOffset? Updated { get; set; }
}
