using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.Services.AuthenticationService;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Services.UserService;
using InTwitter.Views;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.ViewModels
{
    public class MainMasterDetailPageViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPageDialogService _pageDialogService;

        public MainMasterDetailPageViewModel(
            INavigationService navigationService,
            IAuthorizationService authorizationService,
            IAuthenticationService authenticationService,
            IUserService userService,
            IEventAggregator eventAggregator,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _authenticationService = authenticationService;
            _userService = userService;
            _eventAggregator = eventAggregator;
            _pageDialogService = pageDialogService;
            eventAggregator.GetEvent<SelectedTabPageEvent>().Subscribe(SelectTab);
            IsMasterPageGestureEnabled = true;
            Icon = "pic_profile_small.png";
            IsHomeChecked = true;
        }

        #region ---Public properties---

        private bool _isMasterPagePresented;
        public bool IsMasterPagePresented
        {
            get => _isMasterPagePresented;
            set => SetProperty(ref _isMasterPagePresented, value);
        }

        private bool _isMasterPageGestureEnabled;
        public bool IsMasterPageGestureEnabled
        {
            get => _isMasterPageGestureEnabled;
            set => SetProperty(ref _isMasterPageGestureEnabled, value);
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private bool _isHomeChecked;
        public bool IsHomeChecked
        {
            get => _isHomeChecked;
            set => SetProperty(ref _isHomeChecked, value);
        }

        private bool _isSearchChecked;
        public bool IsSearchChecked
        {
            get => _isSearchChecked;
            set => SetProperty(ref _isSearchChecked, value);
        }

        private bool _isNotificationChecked;
        public bool IsNotificationChecked
        {
            get => _isNotificationChecked;
            set => SetProperty(ref _isNotificationChecked, value);
        }

        private bool _isBookmarksChecked;
        public bool IsBookmarksChecked
        {
            get => _isBookmarksChecked;
            set => SetProperty(ref _isBookmarksChecked, value);
        }

        public ICommand OpenProfileTapCommand => SingleExecutionCommand.FromFunc(OnOpenProfileTap);

        public ICommand OpenHomeTabTapCommand => SingleExecutionCommand.FromFunc(OnOpenHomeTabTap);

        public ICommand OpenSearchTabTapCommand => SingleExecutionCommand.FromFunc(OnOpenSearchTabTap);

        public ICommand OpenNotificationTabTapCommand => SingleExecutionCommand.FromFunc(OnOpenNotificationTabTap);

        public ICommand OpenBookmarksTabTapCommand => SingleExecutionCommand.FromFunc(OnOpenBookmarksTabTap);

        public ICommand EditProfileTapCommand => SingleExecutionCommand.FromFunc(OnEditProfileTap);

        public ICommand LogoutProfileTapCommand => SingleExecutionCommand.FromFunc(OnLogoutProfileTap);

        #endregion

        #region ---Overrides---

        public override async void Initialize(INavigationParameters parameters)
        {
            var user = (await _userService.GetUserAsync(_authorizationService.CurrentUserId)).Result;
            var profile = user.ToUserViewModel();

            Icon = profile.IconSource;
            Name = profile.Name;
            Email = profile.Email;
        }

        #endregion

        #region ---Private helpers---

        private void SelectTab(string tab)
        {
            switch (tab)
            {
                case nameof(HomeTabPage):
                    IsHomeChecked = true;
                    break;
                case nameof(SearchTabPage):
                    IsSearchChecked = true;
                    break;
                case nameof(NotificationTabPage):
                    IsNotificationChecked = true;
                    break;
                case nameof(BookmarksTabPage):
                    IsBookmarksChecked = true;
                    break;
            }
        }

        private Task OnOpenProfileTap()
        {
            IsMasterPagePresented = false;
            IsMasterPageGestureEnabled = false;
            _eventAggregator.GetEvent<SelectedMasterMenuItemEvent>().Publish(nameof(ProfilePage));
            return Task.CompletedTask;
        }

        private Task OnOpenHomeTabTap()
        {
            IsMasterPagePresented = false;
            IsHomeChecked = true;
            _eventAggregator.GetEvent<SelectedMasterMenuItemEvent>().Publish(nameof(HomeTabPage));
            return Task.CompletedTask;
        }

        private Task OnOpenSearchTabTap()
        {
            IsMasterPagePresented = false;
            IsSearchChecked = true;
            _eventAggregator.GetEvent<SelectedMasterMenuItemEvent>().Publish(nameof(SearchTabPage));
            return Task.CompletedTask;
        }

        private Task OnOpenNotificationTabTap()
        {
            IsMasterPagePresented = false;
            IsNotificationChecked = true;
            _eventAggregator.GetEvent<SelectedMasterMenuItemEvent>().Publish(nameof(NotificationTabPage));
            return Task.CompletedTask;
        }

        private Task OnOpenBookmarksTabTap()
        {
            IsMasterPagePresented = false;
            IsBookmarksChecked = true;
            _eventAggregator.GetEvent<SelectedMasterMenuItemEvent>().Publish(nameof(BookmarksTabPage));
            return Task.CompletedTask;
        }

        private Task OnEditProfileTap()
        {
            IsMasterPagePresented = false;
            IsMasterPageGestureEnabled = false;
            _eventAggregator.GetEvent<SelectedMasterMenuItemEvent>().Publish(nameof(EditProfilePage));
            return Task.CompletedTask;
        }

        private async Task OnLogoutProfileTap()
        {
            var isLogout = await _pageDialogService.DisplayAlertAsync(
                title: TextResources["LogoutMessage"],
                message: " ",
                acceptButton: TextResources["Confirm"],
                cancelButton: TextResources["Cancel"]);

            if (isLogout)
            {
                _authenticationService.LogOut();
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(CreateAccountFirstPage)}");
            }
        }

        #endregion
    }
}
