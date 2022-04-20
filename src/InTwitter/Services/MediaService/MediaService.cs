using InTwitter.Services.PermissionService;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using InTwitter.Helpers;
using Xamarin.Forms;
using InTwitter.PlatformDependencyInterface;
using System.IO;
using Xamarin.CommunityToolkit.UI.Views;
using System.Diagnostics;

namespace InTwitter.Services.MediaService
{
    public class MediaService : IMediaService
    {
        private readonly IPermissionService _permissionService;

        public MediaService(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<AOResult<string>> PickPhotoFromGalleryAsync()
        {
            var result = new AOResult<string>();

            try
            {
                var status = (await _permissionService.RequestGalleryPermissionAsync()).Result;

                if (status == PermissionStatus.Granted)
                {
                    await _permissionService.RunWithPermission<Permissions.StorageWrite>(async () =>
                    {
                        var saver = DependencyService.Get<IImageSaver>();

                        var photo = await MediaPicker.PickPhotoAsync();

                        var filepath = await saver.Save(photo.FullPath, true);
                        result.SetSuccess(filepath);
                    });
                }
            }
            catch (Exception ex)
            {
                result.SetError(nameof(MediaService), "Something went wrong with photo", ex);
            }

            return result;
        }

        public async Task<AOResult<string>> PickVideoFromGalleryAsync()
        {
            var result = new AOResult<string>();

            try
            {
                var status = (await _permissionService.RequestGalleryPermissionAsync()).Result;

                if (status == PermissionStatus.Granted)
                {
                    var video = await MediaPicker.PickVideoAsync();
                    result.SetSuccess(video.FullPath);
                }
            }
            catch (Exception ex)
            {
                result.SetError(nameof(MediaService), "Something went wrong with video", ex);
            }

            return result;
        }

        public async Task<AOResult<string>> TakingPicturesAsync()
        {
            var result = new AOResult<string>();

            try
            {
                var status = (await _permissionService.RequestCameraPermissionAsync()).Result;

                if (status == PermissionStatus.Granted)
                {
                    var photo = await MediaPicker.CapturePhotoAsync();
                    if (photo != null)
                    {
                        var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                        using (var stream = await photo.OpenReadAsync())
                        using (var newStream = File.OpenWrite(newFile))
                        {
                            await stream.CopyToAsync(newStream);
                        }

                        result.SetSuccess(photo.FullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                result.SetError(nameof(MediaService), "Something went wrong with the photo", ex);
            }

            return result;
        }

        public async Task<AOResult<string>> ShootingVideoAsync()
        {
            var result = new AOResult<string>();
            try
            {
                var status = (await _permissionService.RequestCameraPermissionAsync()).Result;
                if (status == PermissionStatus.Granted)
                {
                    var video = await MediaPicker.CaptureVideoAsync();

                    if (video != null)
                    {
                        result.SetSuccess(video.FullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                result.SetError(nameof(MediaService), "Something went wrong with video", ex);
            }

            return result;
        }

        public async Task<AOResult<bool>> TrimOrRotateVideoAsync(string inputPath, string outputPath, int startMS, int lengthMS = 0, CameraOptions cameraOptions = CameraOptions.Default)
        {
            var result = new AOResult<bool>();
            try
            {
                var saver = DependencyService.Get<IVideoService>();
                var resutOperation = await saver.TrimOrRotateAsync(inputPath, outputPath, startMS, lengthMS, cameraOptions);
                result.SetSuccess(resutOperation);
            }
            catch (Exception ex)
            {
                result.SetError(nameof(MediaService), "Something went wrong with the video cropping.", ex);
            }

            return result;
        }

        public async Task<AOResult<double>> DetermineVideoDurationAsync(string path)
        {
            var result = new AOResult<double>();
            try
            {
                var saver = DependencyService.Get<IVideoService>();
                var resutOperation = await saver.GetVideoLengthAsync(path);
                result.SetSuccess(resutOperation);
            }
            catch (Exception ex)
            {
                result.SetError(nameof(MediaService), "Something went wrong with the video.", ex);
            }

            return result;
        }

        public async Task<AOResult<string>> SaveFromStreamToImage(byte[] data, int offset, int length, bool toTempFolder = false, CameraOptions cameraOptions = CameraOptions.Front)
        {
            var result = new AOResult<string>();

            try
            {
                var status = (await _permissionService.RequestGalleryPermissionAsync()).Result;

                if (status == PermissionStatus.Granted)
                {
                    await _permissionService.RunWithPermission<Permissions.StorageWrite>(async () =>
                    {
                        var saver = DependencyService.Get<IImageSaver>();
                        var filePath = await saver.SaveFromStreamToImage(data, offset, length, toTempFolder, cameraOptions);
                        result.SetSuccess(filePath);
                    });
                }
            }
            catch (Exception ex)
            {
                result.SetError(nameof(MediaService), "Something went wrong with saving the picture.", ex);
            }

            return result;
        }

        public string CreatePath()
        {
            string result = null;

            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + (Device.RuntimePlatform == Device.iOS ? ".mov" : ".mp4");
                string outputPath = Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, fileName);
                result = outputPath;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(nameof(MediaService), "Something went wrong while creating the path.", ex);
            }

            return result;
        }
    }
}