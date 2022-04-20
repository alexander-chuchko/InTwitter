using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Commands;
using InTwitter.Services.AuthenticationService;
using InTwitter.Views;
using InTwitter.Validators;
using Xamarin.Forms;
using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.FeedService;
using InTwitter.Services.AuthorizationService;

namespace InTwitter.ViewModels
{
    public class LogInPageViewModel : ViewModelBase
    {
        private bool _isWaitFocus;
        private IAuthenticationService _authenticationService;

        public LogInPageViewModel(
                                  INavigationService navigationService,
                                  IAuthenticationService authenticationService)
            : base(navigationService)
        {
            _authenticationService = authenticationService;

            TitlePage = TextResources["GetYourAccount"];
            LogInButtonText = TextResources["LogIn"];
            ErrorEmailText = string.Empty;
            ErrorPasswordText = string.Empty;
            IsNormalButtonSize = true;
            SignUpButtonVisibility = true;
            _isWaitFocus = false;
        }

        #region -- Public properties --

        private string titlePage;
        public string TitlePage
        {
            get => titlePage;
            set => SetProperty(ref titlePage, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string errorEmailText;
        public string ErrorEmailText
        {
            get => errorEmailText;
            set => SetProperty(ref errorEmailText, value);
        }

        private bool entryEmailFocused;
        public bool EntryEmailFocused
        {
            get => entryEmailFocused;
            set => SetProperty(ref entryEmailFocused, value);
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

        private string logInButtonText;
        public string LogInButtonText
        {
            get => logInButtonText;
            set => SetProperty(ref logInButtonText, value);
        }

        private bool isNormalButtonSize;
        public bool IsNormalButtonSize
        {
            get => isNormalButtonSize;
            set => SetProperty(ref isNormalButtonSize, value);
        }

        private bool signUpButtonVisibility;
        public bool SignUpButtonVisibility
        {
            get => signUpButtonVisibility;
            set => SetProperty(ref signUpButtonVisibility, value);
        }

        public SingleExecutionCommand LogInButtonCommand => SingleExecutionCommand.FromFunc(OnLogIn);

        public SingleExecutionCommand SignUpCommand => SingleExecutionCommand.FromFunc(OnSignUp);

        public SingleExecutionCommand OutsideTapCommand => SingleExecutionCommand.FromFunc(OnOutsideTapped);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryPasswordFocused) ||
                args.PropertyName == nameof(EntryEmailFocused))
            {
                if (EntryPasswordFocused || EntryEmailFocused)
                {
                    OnEntryFocused();
                }
                else if (!EntryPasswordFocused && !EntryEmailFocused)
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
            else if (args.PropertyName == nameof(Email))
            {
                if (!string.IsNullOrWhiteSpace(Email) &&
                    !string.IsNullOrWhiteSpace(ErrorEmailText))
                {
                    ErrorEmailText = string.Empty;
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue<string>(Constants.NEW_USER_EMAIL, out string email))
            {
                Email = email;
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnEntryUnfocusedAsync()
        {
            _isWaitFocus = true;

            await Task.Delay(20);

            if (_isWaitFocus == true)
            {
                IsNormalButtonSize = true;
                SignUpButtonVisibility = true;
            }

            LogInButtonText = TextResources["LogIn"];
        }

        private void OnEntryFocused()
        {
            _isWaitFocus = false;

            if (IsNormalButtonSize)
            {
                IsNormalButtonSize = false;
                SignUpButtonVisibility = false;
            }

            LogInButtonText = EntryEmailFocused && string.IsNullOrWhiteSpace(Password) ?
                                    TextResources["Next"] : TextResources["LogIn"];
        }

        private async Task OnLogIn()
        {
            if (EntryEmailFocused && string.IsNullOrWhiteSpace(Password))
            {
                LogInButtonText = TextResources["LogIn"];
                EntryPasswordFocused = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorEmailText = string.IsNullOrWhiteSpace(Email) ? TextResources["PleaseTypeYourEmail"] : string.Empty;
                    ErrorPasswordText = string.IsNullOrWhiteSpace(Password) ? TextResources["PleaseTypeYourPassword"] : string.Empty;
                }
                else
                {
                    Email = Email.Trim();
                    Password = Password.Trim();

                    bool validateEmail = Validator.ValidateEmail(Email);
                    bool validatePassword = Validator.ValidatePassword(Password);

                    if (validateEmail && validatePassword)
                    {
                        var result = await _authenticationService.SigInAsync(Email, Password);

                        if (result.IsSuccess)
                        {
                            var res = await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}");
                        }
                        else
                        {
                            ErrorPasswordText = TextResources["WrongPassword"];
                        }
                    }
                    else
                    {
                        ErrorPasswordText = validatePassword ? string.Empty : TextResources["WrongPassword"];
                        ErrorEmailText = validateEmail ? string.Empty : TextResources["WrongEmail"];
                    }
                }
            }
        }

        private async Task OnSignUp()
        {
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(CreateAccountFirstPage)}");
        }

        private async Task OnOutsideTapped()
        {
            DependencyService.Get<IForceKeyboardDismiss>().DismissKeyboard();
            await OnEntryUnfocusedAsync();
            EntryEmailFocused = false;
            EntryPasswordFocused = false;
        }

        #endregion
    }
}