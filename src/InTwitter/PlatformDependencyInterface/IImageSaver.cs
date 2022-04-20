using System.IO;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;

namespace InTwitter.PlatformDependencyInterface
{
    public interface IImageSaver
    {
        Task<string> Save(string uri, bool toTempFolder = false);
        Task<string> SaveFromStreamToImage(byte[] data, int offset, int length, bool toTempFolder = false, CameraOptions cameraOptions = CameraOptions.Front);
    }
}
