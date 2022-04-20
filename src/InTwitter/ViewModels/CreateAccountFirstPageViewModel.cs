using System.ComponentModel;
using System.Threading.Tasks;
using Prism.Navigation;
using InTwitter.Validators;
using InTwitter.Views;
using InTwitter.Helpers;
using Xamarin.Forms;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;

namespace InTwitter.ViewModels
{
    public class CreateAccountFirstPageViewModel : ViewModelBase
    {
        private bool _isWaitFocus;

        public CreateAccountFirstPageViewModel(
                                               INavigationService navigationService)
            : base(navigationService)
        {
            SignUpButtonText = TextResources["SignIn"];
            IsNormalButtonSize = true;
            TitlePage = TextResources["GetYourAccount"];
            LogInButtonVisibility = true;
            ErrorNameText = string.Empty;
            ErrorEmailText = string.Empty;
            _isWaitFocus = false;
        }

        #region --- Public properties ---

        private string titlePage;
        public string TitlePage
        {
            get => titlePage;
            set => SetProperty(ref titlePage, value);
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

        private bool _isNormalButtonSize;
        public bool IsNormalButtonSize
        {
            get => _isNormalButtonSize;
            set => SetProperty(ref _isNormalButtonSize, value);
        }

        private string _signUpButtonText;
        public string SignUpButtonText
        {
            get => _signUpButtonText;
            set => SetProperty(ref _signUpButtonText, value);
        }

        private bool entryNameFocused;
        public bool EntryNameFocused
        {
            get => entryNameFocused;
            set => SetProperty(ref entryNameFocused, value);
        }

        private bool entryEmailFocused;
        public bool EntryEmailFocused
        {
            get => entryEmailFocused;
            set => SetProperty(ref entryEmailFocused, value);
        }

        private string errorNameText;
        public string ErrorNameText
        {
            get => errorNameText;
            set => SetProperty(ref errorNameText, value);
        }

        private string errorEmailText;
        public string ErrorEmailText
        {
            get => errorEmailText;
            set => SetProperty(ref errorEmailText, value);
        }

        private bool logInButtonVisibility;
        public bool LogInButtonVisibility
        {
            get => logInButtonVisibility;
            set => SetProperty(ref logInButtonVisibility, value);
        }

        public SingleExecutionCommand SignUpButtonCommand => SingleExecutionCommand.FromFunc(OnSignUp);

        public SingleExecutionCommand LogInCommand => SingleExecutionCommand.FromFunc(OnLogIn);

        public SingleExecutionCommand OutsideTapCommand => SingleExecutionCommand.FromFunc(OnOutsideTapped);

        #endregion

        #region -- Overrides --

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EntryNameFocused) || args.PropertyName == nameof(EntryEmailFocused))
            {
                if (EntryNameFocused || EntryEmailFocused)
                {
                    OnEntryFocused();
                }
                else if (!EntryNameFocused && !EntryEmailFocused)
                {
                    await OnEntryUnfocusedAsync();
                }
            }
            else if (args.PropertyName == nameof(Name))
            {
                if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(ErrorNameText))
                {
                    ErrorNameText = string.Empty;
                }
            }
            else if (args.PropertyName == nameof(Email))
            {
                if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(ErrorEmailText))
                {
                    ErrorEmailText = string.Empty;
                }
            }
        }

        #endregion

        #region --- Private helpers ---

        private void OnEntryFocused()
        {
            _isWaitFocus = false;

            if (IsNormalButtonSize)
            {
                LogInButtonVisibility = false;
                IsNormalButtonSize = false;
            }

            SignUpButtonText = EntryNameFocused && string.IsNullOrWhiteSpace(Email) ? TextResources["Next"] : TextResources["SignUp"];
            TitlePage = TextResources["CreateYourAccount"];
        }

        private async Task OnEntryUnfocusedAsync()
        {
            _isWaitFocus = true;

            await Task.Delay(10);

            if (_isWaitFocus == true)
            {
                LogInButtonVisibility = true;
                IsNormalButtonSize = true;
            }

            SignUpButtonText = TextResources["SignUp"];
        }

        private async Task OnSignUp()
        {
            if (EntryNameFocused && string.IsNullOrWhiteSpace(Email))
            {
                SignUpButtonText = TextResources["SignUp"];
                EntryEmailFocused = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email))
                {
                    ErrorNameText = string.IsNullOrWhiteSpace(Name) ? TextResources["PleaseTypeYourName"] : string.Empty;
                    ErrorEmailText = string.IsNullOrWhiteSpace(Email) ? TextResources["PleaseTypeYourEmail"] : string.Empty;
                }
                else
                {
                    Name = Name.Trim();
                    Email = Email.Trim();

                    bool validateName = Validator.ValidateName(Name);
                    bool validateEmail = Validator.ValidateEmail(Email);

                    if (validateName && validateEmail)
                    {
                        (string, string) userData = (Name, Email);

                        NavigationParameters parameter = new NavigationParameters
                        {
                            { Constants.NEW_USER_DATA, userData },
                        };

                        await NavigationService.NavigateAsync(nameof(CreateAccountSecondPage), parameter, false, true);
                    }
                    else
                    {
                        ErrorNameText = validateName ? string.Empty : TextResources["WrongName"];
                        ErrorEmailText = validateEmail ? string.Empty : TextResources["WrongEmail"];
                    }
                }
            }
        }

        private async Task OnLogIn()
        {
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(LogInPage)}");
        }

        private async Task OnOutsideTapped()
        {
            DependencyService.Get<IForceKeyboardDismiss>().DismissKeyboard();
            await OnEntryUnfocusedAsync();
            EntryNameFocused = false;
            EntryEmailFocused = false;
        }

        #endregion

    }
}