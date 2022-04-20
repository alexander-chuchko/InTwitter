using InTwitter.Helpers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InTwitter.PlatformDependencyInterface
{
    public interface IGalleryPermissionChecker
    {
        Task<AOResult<PermissionStatus>> CheckStatusAsync();
        Task<AOResult<PermissionStatus>> RequestAsync();
    }
}
