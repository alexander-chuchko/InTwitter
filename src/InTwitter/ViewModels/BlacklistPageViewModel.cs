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
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InTwitter.ViewModels
{
    public class BlacklistPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserInteractionService _userInteractionService;
        private readonly IPageDialogService _pageDialogService;

        public BlacklistPageViewModel(
            INavigationService navigationService,
            IAuthorizationService authorizationService,
            IUserInteractionService userInteractionService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _userInteractionService = userInteractionService;
            _pageDialogService = pageDialogService;
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
            var res = await _userInteractionService.GetUsersInBlacklistAsync(_authorizationService.CurrentUserId);

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
            var isRemoveFromBlacklist = await _pageDialogService.DisplayAlertAsync(
                    title: string.Format(TextResources["RemoveNameFromBlackList"], user.Name),
                    message: " ",
                    acceptButton: TextResources["Remove"],
                    cancelButton: TextResources["Cancel"]);

            if (isRemoveFromBlacklist)
            {
                await _userInteractionService.SetIsUserInBlacklist(_authorizationService.CurrentUserId, user.Id, true);
                UserList.Remove(user);
                SetPageStatus();
            }
        }

        #endregion
    }
}
