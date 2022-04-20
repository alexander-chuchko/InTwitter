using InTwitter.Helpers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InTwitter.PlatformDependencyInterface
{
    public interface ICameraPermissionChecker
    {
        Task<AOResult<PermissionStatus>> GetStatusAsync();
        Task<AOResult<PermissionStatus>> RequestAsync();
    }
}
