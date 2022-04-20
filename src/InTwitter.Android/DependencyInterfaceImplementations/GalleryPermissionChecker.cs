using Xamarin.Forms;
using Xamarin.Essentials;
using InTwitter.PlatformDependencyInterface;
using System.Threading.Tasks;
using InTwitter.Helpers;

[assembly: Dependency(typeof(InTwitter.Droid.DependencyInterfaceImplementations.GalleryPermissionChecker))]
namespace InTwitter.Droid.DependencyInterfaceImplementations
{
    public class GalleryPermissionChecker : IGalleryPermissionChecker
    {
        public async Task<AOResult<PermissionStatus>> CheckStatusAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            result.SetSuccess(status);
            return result;
        }
        public async Task<AOResult<PermissionStatus>> RequestAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            result.SetSuccess(status);
            return result;
        }
    }
}