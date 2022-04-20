using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;

namespace InTwitter.PlatformDependencyInterface
{
    public interface IVideoService
    {
        Task<bool> TrimOrRotateAsync(string inputPath, string outputPath, int startMS, int lengthMS = 0, CameraOptions cameraOptions = CameraOptions.Default);
        Task<double> GetVideoLengthAsync(string path);
    }
}
