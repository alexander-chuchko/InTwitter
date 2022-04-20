using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(InTwitter.iOS.DependencyInterfaceImplementations.GalleryPermissionChecker))]
namespace InTwitter.iOS.DependencyInterfaceImplementations
{
    public class GalleryPermissionChecker : IGalleryPermissionChecker
    {
        public async Task<AOResult<PermissionStatus>> CheckStatusAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
            result.SetSuccess(status);
            return result;
        }

        public async Task<AOResult<PermissionStatus>> RequestAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.RequestAsync<Permissions.Photos>();
            result.SetSuccess(status);
            return result;
        }
    }
}