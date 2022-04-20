using InTwitter.Models.User;
using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.MediaService;
using InTwitter.Services.UserService;
using InTwitter.Validators;
using InTwitter.Views;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using InTwitter.Services.FeedService;

namespace InTwitter.ViewModels
{
    public class EditProfilePageViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMediaService _mediaService;
        private readonly IPageDialogService _pageDialogService;

        private UserViewModel _profile;

        public EditProfilePageViewModel(
            INavigationService navigationService,
            IAuthorizationService authorizationService,
            IUserService userService,
            IMediaService mediaService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _userService = userService;
            _mediaService = mediaService;
            _pageDialogService = pageDialogService;
            NameError = string.Empty;
            EmailError = string.Empty;
            OldPasswordError = string.Empty;
            NewPasswordError = string.Empty;
        }

        #region ---Public properties---

        private string _wallPaper;
        public string WallPaper
        {
            get => _wallPaper;
            set => SetProperty(ref _wallPaper, value);
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private bool _entryNameFocused;
        public bool EntryNameFocused
        {
            get => _entryNameFocused;
            set => SetProperty(ref _entryNameFocused, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _nameError;
        public string NameError
        {
            get => _nameError;
            set => SetProperty(ref _nameError, value);
        }

        private bool _entryEmailFocused;
        public bool EntryEmailFocused
        {
            get => _entryEmailFocused;
            set => SetProperty(ref _entryEmailFocused, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string emailError;
        public string EmailError
        {
            get => emailError;
            set => SetProperty(ref emailError, value);
        }

        private bool _entryOldPasswordFocused;
        public bool EntryOldPasswordFocused
        {
            get => _entryOldPasswordFocused;
            set => SetProperty(ref _entryOldPasswordFocused, value);
        }

        private string oldPassword;
        public string OldPassword
        {
            get => oldPassword;
            set => SetProperty(ref oldPassword, value);
        }

        private string oldPasswordError;
        public string OldPasswordError
        {
            get => oldPasswordError;
            set => SetProperty(ref oldPasswordError, value);
        }

        private bool _entryNewPasswordFocused;
        public bool EntryNewPasswordFocused
        {
            get => _entryNewPasswordFocused;
            set => SetProperty(ref _entryNewPasswordFocused, value);
        }

        private string newPassword;
        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }

        private string newPasswordError;
        public string NewPasswordError
        {
            get => newPasswordError;
            set => SetProperty(ref newPasswordError, value);
        }

        public ICommand OutsideTapCommand => SingleExecutionCommand.FromFunc(OnOutsideTap);

        public ICommand GoBackTapCommand => SingleExecutionCommand.FromFunc(OnGoBackTap);

        public ICommand SavePropfileTapCommand => SingleExecutionCommand.FromFunc(OnSaveProfileTap);

        public ICommand ChangeWallpaperTapCommand => SingleExecutionCommand.FromFunc(OnChangeWallpaperTap);

        public ICommand ChangeIconTapCommand => SingleExecutionCommand.FromFunc(OnChangeIconTap);

        #endregion

        #region ---Overrides---

        public override async void Initialize(INavigationParameters parameters)
        {
            var user = (await _userService.GetUserAsync(_authorizationService.CurrentUserId)).Result;
            _profile = user.ToUserViewModel();

            Name = _profile.Name;
            Email = _profile.Email;
            WallPaper = _profile.WallPapperSource;
            Icon = _profile.IconSource;
        }

        #endregion

        #region ---Private helpers---

        private bool CheckWallpaperChanging()
        {
            bool isWallpaperChanged = false;

            if (!string.IsNullOrWhiteSpace(WallPaper) && !WallPaper.Equals(_profile.WallPapperSource))
            {
                isWallpaperChanged = true;
            }

            return isWallpaperChanged;
        }

        private bool CheckIconChanging()
        {
            bool isIconChanged = false;

            if (!string.IsNullOrWhiteSpace(Icon) && !Icon.Equals(_profile.IconSource))
            {
                isIconChanged = true;
            }

            return isIconChanged;
        }

        private bool CheckNameChanging()
        {
            bool isNameChanged = false;

            if (!Name.Equals(_profile.Name))
            {
                isNameChanged = true;
            }

            return isNameChanged;
        }

        private bool ValidateName()
        {
            bool isRightName = false;

            if (string.IsNullOrWhiteSpace(Name))
            {
                NameError = "Enter your name";
            }
            else if (!Validator.ValidateName(Name))
            {
                NameError = "Wrong name";
            }
            else
            {
                isRightName = true;
            }

            return isRightName;
        }

        private bool CheckEmailChanging()
        {
            bool isEmailChanged = false;

            if (!Email.Equals(_profile.Email))
            {
                isEmailChanged = true;
            }

            return isEmailChanged;
        }

        private bool ValidateEmail()
        {
            bool isRightEmail = false;

            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Enter your email";
            }
            else if (!Validator.ValidateEmail(Email))
            {
                EmailError = "Wrong email";
            }
            else
            {
                isRightEmail = true;
            }

            return isRightEmail;
        }

        private bool CheckPasswordChanging()
        {
            bool isPasswordChanged = false;

            if (!string.IsNullOrWhiteSpace(OldPassword) || !string.IsNullOrWhiteSpace(NewPassword))
            {
                isPasswordChanged = true;
            }

            return isPasswordChanged;
        }

        private bool ValidatePassword()
        {
            bool isRightPassword = false;

            if (string.IsNullOrWhiteSpace(OldPassword))
            {
                OldPasswordError = "Enter your password";
            }
            else if (!OldPassword.Equals(_profile.HashPassword))
            {
                OldPasswordError = "Wrong password";
            }
            else if (string.IsNullOrWhiteSpace(NewPassword))
            {
                NewPasswordError = "Enter new password";
            }
            else if (!Validator.ValidatePassword(NewPassword))
            {
                NewPasswordError = "Wrong password";
            }
            else if (NewPassword.Equals(OldPassword))
            {
                NewPasswordError = "New password is the same as the old one";
            }
            else
            {
                isRightPassword = true;
            }

            return isRightPassword;
        }

        private bool CheckAllChanging()
        {
            bool isProfileChanged = false;
            bool isWallPaperChanged = CheckWallpaperChanging();
            bool isIconChanged = CheckIconChanging();
            bool isNameChanged = CheckNameChanging();
            bool isEmailChanged = CheckEmailChanging();
            bool isPasswordChanged = CheckPasswordChanging();
            NameError = string.Empty;
            EmailError = string.Empty;
            OldPasswordError = string.Empty;
            NewPassword = string.Empty;

            if (isWallPaperChanged || isIconChanged || isNameChanged || isEmailChanged || isPasswordChanged)
            {
                isProfileChanged = true;

                if (isNameChanged && !ValidateName())
                {
                    isProfileChanged = false;
                }

                if (isEmailChanged && !ValidateEmail())
                {
                    isProfileChanged = false;
                }

                if (isPasswordChanged && !ValidatePassword())
                {
                    isProfileChanged = false;
                }
            }

            return isProfileChanged;
        }

        private async Task OnChangeWallpaperTap()
        {
            WallPaper = (await _mediaService.PickPhotoFromGalleryAsync()).Result;
        }

        private async Task OnChangeIconTap()
        {
            Icon = (await _mediaService.PickPhotoFromGalleryAsync()).Result;
        }

        private Task OnOutsideTap()
        {
            DependencyService.Get<IForceKeyboardDismiss>().DismissKeyboard();
            EntryNameFocused = false;
            EntryEmailFocused = false;
            EntryOldPasswordFocused = false;
            EntryNewPasswordFocused = false;
            return Task.CompletedTask;
        }

        private async Task OnGoBackTap()
        {
            (Application.Current.MainPage as MainMasterDetailPage).IsGestureEnabled = true;
            await NavigationService.GoBackAsync();
        }

        private async Task OnSaveProfileTap()
        {
            bool isProfileChanged = CheckAllChanging();

            if (isProfileChanged)
            {
                var isChangeProfile = await _pageDialogService.DisplayAlertAsync(
                    title: "Question",
                    message: "Are you sure that you want to change profile?",
                    acceptButton: "Confirm",
                    cancelButton: "Cancel");

                if (isChangeProfile)
                {
                    bool isWallPaperChanged = CheckWallpaperChanging();
                    bool isIconChanged = CheckIconChanging();
                    bool isNameChanged = CheckNameChanging();
                    bool isEmailChanged = CheckEmailChanging();
                    bool isPasswordChanged = CheckPasswordChanging();

                    if (isWallPaperChanged)
                    {
                        await _userService.ChangeWallPaperAsync(_authorizationService.CurrentUserId, WallPaper);
                    }

                    if (isIconChanged)
                    {
                        await _userService.ChangeIconAsync(_authorizationService.CurrentUserId, Icon);
                    }

                    if (isNameChanged)
                    {
                        await _userService.ChangeNameAsync(_authorizationService.CurrentUserId, Name);
                    }

                    if (isEmailChanged)
                    {
                        await _userService.ChangeEmailAsync(_authorizationService.CurrentUserId, Email);
                    }

                    if (isPasswordChanged)
                    {
                        await _userService.ChangePasswordAsync(_authorizationService.CurrentUserId, NewPassword);
                    }

                    await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}/{nameof(ProfilePage)}");
                }
            }
        }

        #endregion
    }
}
