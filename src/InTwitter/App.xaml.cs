using Prism.Ioc;
using Xamarin.Forms;
using InTwitter.Services.AuthenticationService;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Services.MediaService;
using InTwitter.Services.SearchService;
using InTwitter.Services.SettingsManager;
using InTwitter.Services.TweetService;
using InTwitter.Services.PermissionService;
using InTwitter.Services.UserInteractionService;
using InTwitter.Services.UserService;
using InTwitter.ViewModels;
using InTwitter.Views;
using InTwitter.Services.StoriesService;
using InTwitter.Services.Repository;

namespace InTwitter
{
    public partial class App
    {
        private IAuthorizationService _authorizationService;
        public IAuthorizationService AuthorizationService => _authorizationService ??= Container.Resolve<IAuthorizationService>();

        private IAuthenticationService _authenticationService;
        public IAuthenticationService AuthenticationService => _authenticationService ??= Container.Resolve<IAuthenticationService>();

        private ISettingsManager _settingsManager;
        public ISettingsManager SettingsManager => _settingsManager ??= Container.Resolve<ISettingsManager>();

        public App()
        {
        }

        protected override async void OnInitialized()
        {
            this.InitializeComponent();

            Mocks.MocksGenerator.GenerateMocks();

            if (AuthenticationService.HasSessionToken())
            {
                await AuthenticationService.SigUpAsync("Admin", "admin@mail.com", "Password123");
                await AuthenticationService.SigInAsync("admin@mail.com", "Password123");

                await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}");
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(CreateAccountFirstPage)}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISettingsManager>(Container.Resolve<SettingsManager>);
            containerRegistry.Register<IAuthorizationService>(Container.Resolve<AuthorizationService>);
            containerRegistry.Register<IAuthenticationService>(Container.Resolve<AuthenticationService>);
            containerRegistry.Register<ITweetService>(Container.Resolve<TweetService>);
            containerRegistry.Register<IFeedService>(Container.Resolve<FeedService>);
            containerRegistry.Register<IUserInteractionService>(Container.Resolve<UserInteractionService>);
            containerRegistry.Register<IPermissionService>(Container.Resolve<PermissionService>);
            containerRegistry.Register<IMediaService>(Container.Resolve<MediaService>);
            containerRegistry.Register<IUserService>(Container.Resolve<UserService>);
            containerRegistry.Register<ISearchService>(Container.Resolve<SearchService>);
            containerRegistry.Register<IStoriesService>(Container.Resolve<StoriesService>);
            containerRegistry.Register<IRepository>(Container.Resolve<Repository>);

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<EditProfilePage, EditProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<HomeTabPage, FeedViewModel>();

            containerRegistry.RegisterForNavigation<CreateAccountFirstPage, CreateAccountFirstPageViewModel>();
            containerRegistry.RegisterForNavigation<CreateAccountSecondPage, CreateAccountSecondPageViewModel>();
            containerRegistry.RegisterForNavigation<LogInPage, LogInPageViewModel>();

            containerRegistry.RegisterForNavigation<TweetPage, TweetPageViewModel>();
            containerRegistry.RegisterForNavigation<TweetMediaPage, TweetMediaPageViewModel>();
            containerRegistry.RegisterForNavigation<MainMasterDetailPage, MainMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<SearchTabPage, SearchTabPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationTabPage, NotificationTabPageViewModel>();
            containerRegistry.RegisterForNavigation<BookmarksTabPage, BookmarksTabPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<AddPostPage, AddPostPageViewModel>();
            containerRegistry.RegisterForNavigation<BlacklistPage, BlacklistPageViewModel>();
            containerRegistry.RegisterForNavigation<MutedListPage, MutedListPageViewModel>();
            containerRegistry.RegisterForNavigation<StoriesPage, StoriesPageViewModel>();
            containerRegistry.RegisterForNavigation<CameraPage, CameraPageViewModel>();
        }
    }
}
