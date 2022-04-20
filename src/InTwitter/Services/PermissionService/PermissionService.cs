using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace InTwitter.Services.PermissionService
{
    public class PermissionService : IPermissionService
    {
        private readonly IGalleryPermissionChecker _galleryPermissionChecker;
        private readonly ICameraPermissionChecker _cameraPermissionChecker;
        private readonly IMicrophonePermissionChecker _microphonePermissionChecker;

        public PermissionService()
        {
            _galleryPermissionChecker = DependencyService.Get<IGalleryPermissionChecker>();
            _cameraPermissionChecker = DependencyService.Get<ICameraPermissionChecker>();
            _microphonePermissionChecker = DependencyService.Get<IMicrophonePermissionChecker>();
        }

        public async Task<AOResult<PermissionStatus>> RequestCameraPermissionAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = (await _cameraPermissionChecker.GetStatusAsync()).Result;

            if (status != PermissionStatus.Granted)
            {
                status = (await _cameraPermissionChecker.RequestAsync()).Result;
            }

            result.SetSuccess(status);
            return result;
        }

        public async Task<AOResult<PermissionStatus>> RequestGalleryPermissionAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = (await _galleryPermissionChecker.CheckStatusAsync()).Result;

            if (status != PermissionStatus.Granted)
            {
                status = (await _galleryPermissionChecker.RequestAsync()).Result;
            }

            result.SetSuccess(status);
            return result;
        }

        public async Task<AOResult<PermissionStatus>> RequestMicrophonePermissionAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = (await _microphonePermissionChecker.GetStatusAsync()).Result;
            if (status != PermissionStatus.Granted)
            {
                status = (await _microphonePermissionChecker.RequestAsync()).Result;
            }

            result.SetSuccess(status);
            return result;
        }

        public async Task<AOResult> RunWithPermission<TPermission>(Func<Task> function)
            where TPermission : BasePermission, new()
        {
            var result = new AOResult();

            var status = await CheckStatusAsync<TPermission>();

            if (status != PermissionStatus.Granted)
            {
                status = await RequestAsync<TPermission>();
            }

            if (status == PermissionStatus.Granted)
            {
                await function();
                result.SetSuccess();
            }

            return result;
        }
    }
}
