using Foundation;
using ImageIO;
using InTwitter.PlatformDependencyInterface;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Dependency(typeof(InTwitter.iOS.DependencyInterfaceImplementations.ImageSaver))]
namespace InTwitter.iOS.DependencyInterfaceImplementations
{
    public class ImageSaver : IImageSaver
    {
        public async Task<string> Save(string uri, bool toTempFolder = false)
        {
            var handler = new ImageLoaderSourceHandler();
            UIKit.UIImage bit;
            if (uri.StartsWith('/'))
            {
                bit = UIKit.UIImage.FromFile(uri);
            }
            else
            {
                bit = await handler.LoadImageAsync(ImageSource.FromUri(new Uri(uri)));
            }

            var path = "";
            if (toTempFolder == false)
            {
                bit.SaveToPhotosAlbum(new UIKit.UIImage.SaveStatus((image, error) =>
                {
                    if(error == null)
                    {
                        path = "completed";
                    }
                }));
            }
            else
            {
                var data = bit.AsJPEG();
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Guid.NewGuid().ToString().Replace('-', 'Z')}.png");
                data.Save(path, true);
            }
            await Task.Delay(1000);
            return path;
        }

        public async Task<string> SaveFromStreamToImage(byte[] data, int offset, int length, bool toTempFolder = false, CameraOptions cameraOptions = CameraOptions.Front)
        {
            var path = string.Empty;
            try
            {
                UIKit.UIImage bit = new UIKit.UIImage(NSData.FromArray(data));


                var imageObject = bit.AsJPEG();

                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Guid.NewGuid().ToString().Replace('-', 'Z')}.png");

                imageObject.Save(path, true);

                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong in { nameof(SaveFromStreamToImage)}");
            }

            return path;
        }
    }
}
