using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(InTwitter.Droid.DependencyInterfaceImplementations.MicrophonePermissionChecker))]
namespace InTwitter.Droid.DependencyInterfaceImplementations
{
    public class MicrophonePermissionChecker : IMicrophonePermissionChecker
    {
        public async Task<AOResult<PermissionStatus>> GetStatusAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            result.SetSuccess(status);
            return result;
        }

        public async Task<AOResult<PermissionStatus>> RequestAsync()
        {
            var result = new AOResult<PermissionStatus>();
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            result.SetSuccess(status);
            return result;
        }
    }
}