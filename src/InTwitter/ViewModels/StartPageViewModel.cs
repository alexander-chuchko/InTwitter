using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;
using InTwitter.Mocks;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Extensions;
using InTwitter.Models.Tweet;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;

namespace InTwitter.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public StartPageViewModel(
                                  INavigationService navigationService)
         : base(navigationService)
        {
        }
    }
}
