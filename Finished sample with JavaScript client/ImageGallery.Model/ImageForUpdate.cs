using System.ComponentModel.DataAnnotations;

namespace ImageGallery.Model;

public class ImageForUpdate(string title)
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = title;
}
