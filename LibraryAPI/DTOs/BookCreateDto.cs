using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class BookCreateDto
{
    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Author { get; set; } = null!;

    [Required]
    [StringLength(13)]
    [JsonPropertyName("isbn")]   // <-- ensures JSON mapping
    public string Isbn { get; set; } = null!;

    public int? PublishedYear { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int AvailableCopies { get; set; }
}
