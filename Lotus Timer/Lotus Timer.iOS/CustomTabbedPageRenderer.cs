using System;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Svg.Platform;
using TabbedPageSVGIcons;
using TabbedPageSVGIcons.iOS;
using UIKit;
using Lotus_Timer.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabsPage), typeof(CustomTabbedPageRenderer))]
namespace TabbedPageSVGIcons.iOS
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        protected override async Task<Tuple<UIImage, UIImage>> GetIcon(Page page)
        {
            UIImage imageIcon;

            if (page.IconImageSource is FileImageSource fileImage && fileImage.File.Contains(".svg"))
            {
                // Load SVG from file
                UIImage uiImage = await ImageService.Instance.LoadFile(fileImage.File)
                    .WithCustomDataResolver(new SvgDataResolver(15, 15, true))
                    .AsUIImageAsync();
                imageIcon = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            }
            else
            {
                // Load Xamarin.Forms supported image
                IImageSourceHandler sourceHandler = null;
                if (page.IconImageSource is UriImageSource)
                    sourceHandler = new ImageLoaderSourceHandler();
                else if (page.IconImageSource is FileImageSource)
                    sourceHandler = new FileImageSourceHandler();
                else if (page.IconImageSource is StreamImageSource)
                    sourceHandler = new StreamImagesourceHandler();
                else if (page.IconImageSource is FontImageSource)
                    sourceHandler = new FontImageSourceHandler();

                UIImage uiImage = await sourceHandler.LoadImageAsync(page.IconImageSource);
                imageIcon = uiImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            }

            return new Tuple<UIImage, UIImage>(imageIcon, null);
        }
    }
}