using ImageGallery.Model;

namespace ImageGallery.Client.ViewModels;

public class GalleryIndexViewModel(IEnumerable<Image> images)
{
    public IEnumerable<Image> Images { get; private set; }  = images;
}
