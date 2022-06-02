using BookSearch.API.Abstracts;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSearch.API.DDD.Book
{
    [Table("books")]
    public record Book : BaseModel
    {
        [Column("title")]
        [Required]
        public string Title { get; set; } = string.Empty;

        [Column("subtitle")]
        public string? Subtitle { get; set; }

        [Column("language")]
        public string? Language { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("page_count")]
        public int? PageCount { get; set; }

        [Column("publisher")]
        public string? Publisher { get; set; }

        [Column("publication_date")]
        public string? PublicationDate { get; set; }

        [Column("image")]
        public string? Image { get; set; } 

        [Column("average_ratting")]
        public double? AverageRating { get; set; }

        public virtual List<Identifier.Identifier> Identifiers { get; set; } = default!;

        public virtual List<Author.Author> Authors { get; set; } = default!;

        public virtual List<Category.Category> Categories { get; set; } = default!;

        public virtual List<Favorite.Favorite> Favorites { get; set; } = default!;
    }
}
