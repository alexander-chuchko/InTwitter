using System.Threading.Tasks;
using InTwitter.Helpers;
using Xamarin.CommunityToolkit.UI.Views;

namespace InTwitter.Services.MediaService
{
    public interface IMediaService
    {
        string CreatePath();
        Task<AOResult<string>> PickPhotoFromGalleryAsync();
        Task<AOResult<string>> PickVideoFromGalleryAsync();
        Task<AOResult<string>> TakingPicturesAsync();

        Task<AOResult<bool>> TrimOrRotateVideoAsync(string inputPath, string outputPath, int startMS, int lengthMS = 0, CameraOptions cameraOptions = CameraOptions.Default);

        Task<AOResult<double>> DetermineVideoDurationAsync(string path);

        Task<AOResult<string>> SaveFromStreamToImage(byte[] data, int offset, int length, bool toTempFolder = false, CameraOptions cameraOptions = CameraOptions.Front);
    }
}
