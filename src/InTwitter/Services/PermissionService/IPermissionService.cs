using InTwitter.Helpers;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InTwitter.Services.PermissionService
{
    public interface IPermissionService
    {
        Task<AOResult<PermissionStatus>> RequestGalleryPermissionAsync();
        Task<AOResult<PermissionStatus>> RequestCameraPermissionAsync();
        Task<AOResult<PermissionStatus>> RequestMicrophonePermissionAsync();
        Task<AOResult> RunWithPermission<TPermission>(Func<Task> function)
             where TPermission : Permissions.BasePermission, new();
    }
}
