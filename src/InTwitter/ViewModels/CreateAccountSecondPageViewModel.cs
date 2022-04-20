using System;
using System.ComponentModel;
using System.Threading.Tasks;
using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.AuthenticationService;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Validators;
using InTwitter.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace InTwitter.ViewModels
{
    public class CreateAccountSecondPageViewModel : ViewModelBase
    {
        private bool _isWaitFocus;
        private IAuthenticationService _authenticationService;

        public CreateAccountSecondPageViewModel(
            INavigationService navigationService,
            IAuthenticationService authenticationService)
            : base(navigationService)
        {
            _authenticationService = authenticationService;

            TitlePage = TextResources["YouNeedPassword"];
            ConfirmButtonText = TextResources["Confirm"];
            IsNormalButtonSize = true;
            ErrorPasswordText = string.Empty;
            ErrorConfirmPasswordText = string.Empty;
            _isWaitFocus = false;
        }

        #region -- Public Properties --

        private string titlePage;
        public string TitlePage
        {
            get => titlePage;
            set => SetProperty(ref titlePage, value);
        }

        private string confirmButtonText;
        public string ConfirmButtonText
        {
            get => confirmButtonText;
            set => SetProperty(ref confirmButtonText, value);
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        private string errorPasswordText;
        public string ErrorPasswordText
        {
            get => errorPasswordText;
            set => SetProperty(ref errorPasswordText, value);
        }

        private bool entryPasswordFocused;
        public bool EntryPasswordFocused
        {
            get => entryPasswordFocused;
            set => SetProperty(ref entryPasswordFocused, value);
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        private string errorConfirmPasswordText;
        public string ErrorConfirmPasswordText
        {
            get => errorConfirmPasswordText;
            set => SetProperty(ref errorConfirmPasswordText, value);
        }

        private bool entryConfirmPasswordFocused;
        public bool EntryConfirmPasswordFocused
        {
            get => entryConfirmPasswordFocused;
            set => SetProperty(ref entryConfirmPasswordFocused, value);
        }

        private bool isNormalButtonSize;
        public bool IsNormalButtonSize
        {
            get => isNormalButtonSize;
            set => SetProperty(ref isNormalButtonSize, value);
        }

        private string name;
        public string Name
        {
            get => name;
            private set => name = value;
        }

        private string email;
        public string Email
        {
            get => email;
            private set => email = value;
        }

        public SingleExecutionCommand ConfirmButtonCommand => SingleExecutionCommand.FromFunc(OnConfirm);

        public SingleExecutionCommand GoBackCommand => SingleExecutionCommand.FromFunc(OnGoBack);

        public SingleExecutionCommand OutsideTapCommand => SingleExecutionCommand.FromFunc(OnOutsideTapped);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryPasswordFocused) ||
                args.PropertyName == nameof(EntryConfirmPasswordFocused))
            {
                if (EntryPasswordFocused || EntryConfirmPasswordFocused)
                {
                    OnEntryFocused();
                }
                else if (!EntryPasswordFocused && !EntryConfirmPasswordFocused)
                {
                    OnEntryUnfocusedAsync();
                }
            }
            else if (args.PropertyName == nameof(Password))
            {
                if (!string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(ErrorPasswordText))
                {
                    ErrorPasswordText = string.Empty;
                }
            }
            else if (args.PropertyName == nameof(ConfirmPassword))
            {
                if (!string.IsNullOrWhiteSpace(ConfirmPassword) &&
                    !string.IsNullOrWhiteSpace(ErrorConfirmPasswordText))
                {
                    ErrorConfirmPasswordText = string.Empty;
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue<(string, string)>(Constants.NEW_USER_DATA, out (string, string) userData))
            {
                Name = userData.Item1;
                Email = userData.Item2;
            }
        }

        #endregion

        #region -- Private helpers --

        private void OnEntryFocused()
        {
            _isWaitFocus = false;

            if (IsNormalButtonSize)
            {
                IsNormalButtonSize = false;
            }

            ConfirmButtonText = EntryPasswordFocused && string.IsNullOrWhiteSpace(ConfirmPassword) ?
                                    TextResources["Next"] : TextResources["Confirm"];
        }

        private async Task OnEntryUnfocusedAsync()
        {
            _isWaitFocus = true;

            await Task.Delay(20);
            if (_isWaitFocus == true)
            {
                IsNormalButtonSize = true;
            }

            ConfirmButtonText = TextResources["Confirm"];
        }

        private async Task OnConfirm()
        {
            if (EntryPasswordFocused && string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmButtonText = TextResources["Confirm"];
                EntryConfirmPasswordFocused = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    ErrorPasswordText = string.IsNullOrWhiteSpace(Password) ? TextResources["PleaseTypePassword"] : string.Empty;
                    ErrorConfirmPasswordText = string.IsNullOrWhiteSpace(ConfirmPassword) ? TextResources["PleaseConfirmPassword"] :
                        string.Empty;
                }
                else
                {
                    Password = Password.Trim();
                    ConfirmPassword = ConfirmPassword.Trim();

                    bool validatePassword = Validator.ValidatePassword(Password);
                    bool passwordsMatch = Password == ConfirmPassword;

                    if (validatePassword && passwordsMatch)
                    {
                        var result = await _authenticationService.SigUpAsync(Name, Email, Password);

                        if (result.IsSuccess)
                        {
                            NavigationParameters parameter = new NavigationParameters
                        {
                            { Constants.NEW_USER_EMAIL, Email },
                        };

                            await NavigationService.NavigateAsync($"/{nameof(LogInPage)}", parameter);
                        }
                    }
                    else
                    {
                        ErrorPasswordText = validatePassword ? string.Empty : TextResources["WrongPassword"];
                        ErrorConfirmPasswordText = passwordsMatch ? string.Empty : TextResources["DontMatch"];
                    }
                }
            }
        }

        private async Task OnGoBack()
        {
            await NavigationService.GoBackAsync();
        }

        private async Task OnOutsideTapped()
        {
            DependencyService.Get<IForceKeyboardDismiss>().DismissKeyboard();
            await OnEntryUnfocusedAsync();
            EntryPasswordFocused = false;
            EntryConfirmPasswordFocused = false;
        }

        #endregion
    }
}