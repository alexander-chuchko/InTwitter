using InTwitter.Helpers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InTwitter.PlatformDependencyInterface
{
    public interface IMicrophonePermissionChecker
    {
        Task<AOResult<PermissionStatus>> RequestAsync();
        Task<AOResult<PermissionStatus>> GetStatusAsync();
    }
}
