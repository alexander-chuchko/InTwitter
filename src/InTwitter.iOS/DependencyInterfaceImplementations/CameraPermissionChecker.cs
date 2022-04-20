using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(InTwitter.iOS.DependencyInterfaceImplementations.CameraPermissionChecker))]
namespace InTwitter.iOS.DependencyInterfaceImplementations
{
    public class CameraPermissionChecker : ICameraPermissionChecker
    {
        public async Task<AOResult<PermissionStatus>> GetStatusAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            result.SetSuccess(status);
            return result;
        }

        public async Task<AOResult<PermissionStatus>> RequestAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            result.SetSuccess(status);
            return result;
        }
    }
}