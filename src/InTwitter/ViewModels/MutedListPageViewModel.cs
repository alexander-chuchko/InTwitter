using InTwitter.Enums;
using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.Models.User;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.UserInteractionService;
using InTwitter.Views;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InTwitter.ViewModels
{
    public class MutedListPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IUserInteractionService _userInteractionService;

        public MutedListPageViewModel(
            INavigationService navigationService,
            IAuthorizationService authorizationService,
            IPageDialogService pageDialogService,
            IUserInteractionService userInteractionService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _pageDialogService = pageDialogService;
            _userInteractionService = userInteractionService;
        }

        #region ---Public properties---

        private ObservableCollection<UserViewModel> _usersList;
        public ObservableCollection<UserViewModel> UserList
        {
            get => _usersList;
            set => SetProperty(ref _usersList, value);
        }

        private EListPageStatus _listStatus;
        public EListPageStatus ListStatus
        {
            get => _listStatus;
            set => SetProperty(ref _listStatus, value);
        }

        public ICommand GoBackTapCommand => SingleExecutionCommand.FromFunc(OnGoBackTap);

        public ICommand OpenProfileTapCommand => SingleExecutionCommand.FromFunc<UserViewModel>(OnOpenProfileTap);

        public ICommand RemoveProfileTapCommand => SingleExecutionCommand.FromFunc<UserViewModel>(OnRemoveProfileTap);

        #endregion

        #region ---Overrides---

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var res = await _userInteractionService.GetMutedUsersAsync(_authorizationService.CurrentUserId);

            if (res.IsSuccess)
            {
                List<UserViewModel> uvm = new List<UserViewModel>();

                foreach (var item in res.Result)
                {
                    uvm.Add(item.ToUserViewModel());
                }

                UserList = new ObservableCollection<UserViewModel>(uvm);
            }

            SetPageStatus();
        }

        #endregion

        #region ---Private helpers---

        private void SetPageStatus()
        {
            if (UserList is null || UserList.Count == 0)
            {
                ListStatus = EListPageStatus.Empty;
            }
            else
            {
                ListStatus = EListPageStatus.Fill;
            }
        }

        private async Task OnGoBackTap()
        {
            await NavigationService.GoBackAsync();
        }

        private async Task OnOpenProfileTap(UserViewModel user)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(User), user.Id);
            await NavigationService.NavigateAsync(nameof(ProfilePage), parameters);
        }

        private async Task OnRemoveProfileTap(UserViewModel user)
        {
            var isRemoveFromMutelist = await _pageDialogService.DisplayAlertAsync(
                                            title: string.Format(TextResources["RemoveFromMuteMessage"], user.Name),
                                            message: " ",
                                            acceptButton: TextResources["Remove"],
                                            cancelButton: TextResources["Cancel"]);

            if (isRemoveFromMutelist)
            {
                await _userInteractionService.SetIsUserMuted(_authorizationService.CurrentUserId, user.Id, true);
                UserList.Remove(user);
                SetPageStatus();
            }
        }

        #endregion

    }
}
