using InTwitter.Models.User;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Views;
using Prism.Events;
using Prism.Navigation;
using System.ComponentModel;

namespace InTwitter.ViewModels
{
    public class MainTabbedPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAuthorizationService _authorizationService;

        public MainTabbedPageViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<SelectedMasterMenuItemEvent>().Subscribe(SelectTab);
        }

        #region ---Public properties---

        private string _chosenTab;
        public string ChosenTab
        {
            get => _chosenTab;
            set => SetProperty(ref _chosenTab, value);
        }

        private bool _isGestureEnabled = false;
        public bool IsGestureEnabled
        {
            get => _isGestureEnabled;
            set => SetProperty(ref _isGestureEnabled, value);
        }

        #endregion

        #region ---Overrides---

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(ChosenTab))
            {
                _eventAggregator.GetEvent<SelectedTabPageEvent>().Publish(ChosenTab);
            }
        }

        #endregion

        #region ---Private helpers---

        private async void SelectTab(string tab)
        {
            if (tab == nameof(ProfilePage))
            {
                var parameters = new NavigationParameters();
                parameters.Add(nameof(User), _authorizationService.CurrentUserId);
                await NavigationService.NavigateAsync(tab, parameters);
            }
            else if (tab == nameof(EditProfilePage))
            {
                await NavigationService.NavigateAsync(tab);
            }
            else
            {
                ChosenTab = tab;
            }
        }

        #endregion
    }
}
