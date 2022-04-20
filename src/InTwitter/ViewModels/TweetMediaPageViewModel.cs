using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Prism.Navigation;
using InTwitter.Models.Tweet;
using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.TweetService;
using InTwitter.Services.PermissionService;
using InTwitter.Services.FeedService;

namespace InTwitter.ViewModels
{
    public class TweetMediaPageViewModel : ViewModelBase
    {
        private IAuthorizationService _authorizationService;

        private IPermissionService _permissionService;

        private ITweetService _tweetService;

        public TweetMediaPageViewModel(
                                       INavigationService navigationService,
                                       IAuthorizationService authorizationService,
                                       ITweetService tweetService,
                                       IPermissionService permissionService)
            : base(navigationService)
        {
            this._authorizationService = authorizationService;
            this._tweetService = tweetService;
            this._permissionService = permissionService;
        }

        #region ---Public Properties---

        private int _carouselPosition;
        public int CarouselPosition
        {
            get => _carouselPosition;
            set
            {
                SetProperty(ref _carouselPosition, value);
                RaisePropertyChanged(nameof(CarouselFormatedPosition));
            }
        }

        private TweetViewModel _tweet;
        public TweetViewModel Tweet
        {
            get => _tweet;
            set => SetProperty(ref _tweet, value);
        }

        private bool _isDropDownExpanded;
        public bool IsDropDownExpanded
        {
            get => _isDropDownExpanded;
            set => SetProperty(ref _isDropDownExpanded, value);
        }

        public string CarouselFormatedPosition => $"{CarouselPosition + 1} – {Tweet?.MediaSources.Count}";

        public ICommand LikeTappedCommand => SingleExecutionCommand.FromFunc(LikeTappedHandler);

        public ICommand MarkTappedCommand => SingleExecutionCommand.FromFunc(MarkTappedHandler);

        public ICommand OnPageTapCommand => SingleExecutionCommand.FromFunc(OnPageTapHandler);

        public ICommand OnDetailTapCommand => SingleExecutionCommand.FromFunc(OnDetailTapHandler);

        public ICommand SaveImageCommand => SingleExecutionCommand.FromFunc(SaveImageHandler);

        public ICommand ShareImageCommand => SingleExecutionCommand.FromFunc(ShareImageHandler);

        #endregion

        #region ---Overrides---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IChangerStatusBar>().ChangeTitleColor(false);
                DependencyService.Get<IChangerStatusBar>().ChangeStatusBarColor(Color.FromHex("#02060E"));
            }

            if (parameters.ContainsKey(nameof(TweetViewModel)))
            {
                Tweet = parameters[nameof(TweetViewModel)] as TweetViewModel;

                CarouselPosition = (int)parameters["position"];
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IChangerStatusBar>().ChangeTitleColor(true);
                DependencyService.Get<IChangerStatusBar>().ChangeStatusBarColor(Color.FromHex("#FFF"));
            }
        }

        #endregion

        #region ---Private Helpers---

        private async Task SaveImageHandler()
        {
            IsDropDownExpanded = false;

            if (Device.RuntimePlatform == Device.Android)
            {
                await _permissionService.RunWithPermission<Permissions.StorageWrite>(async () =>
                {
                    await saveImage();
                });
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                await _permissionService.RunWithPermission<Permissions.Photos>(async () =>
                {
                    await saveImage();
                });
            }

            async Task saveImage()
            {
                var saver = DependencyService.Get<IImageSaver>();
                var filepath = await saver.Save(Tweet.MediaSources[CarouselPosition].MediaSource);

                var snackBar = DependencyService.Get<ISnackbarShower>();
                if (string.IsNullOrEmpty(filepath))
                {
                    snackBar.Show(TextResources["UnableSaveImageMessage"]);
                }
                else
                {
                    snackBar.Show(TextResources["SuccessSaveImageMessage"]);
                }
            }
        }

        private async Task ShareImageHandler()
        {
            IsDropDownExpanded = false;

            var saver = DependencyService.Get<IImageSaver>();
            var filepath = await saver.Save(Tweet.MediaSources[CarouselPosition].MediaSource, true);

            if (string.IsNullOrEmpty(filepath))
            {
                var snackBar = DependencyService.Get<ISnackbarShower>();
                snackBar.Show(TextResources["UnableShareImageMessage"]);
            }
            else
            {
                await Share.RequestAsync(new ShareFileRequest(new ShareFile(filepath)));
            }
        }

        private Task OnDetailTapHandler()
        {
            IsDropDownExpanded = true;

            return Task.CompletedTask;
        }

        private Task OnPageTapHandler()
        {
            IsDropDownExpanded = false;

            return Task.CompletedTask;
        }

        private async Task LikeTappedHandler()
        {
            Tweet.IsUserLiked = !Tweet.IsUserLiked;
            Tweet.LikesAmount += Tweet.IsUserLiked ? +1 : -1;

            await _tweetService.SetIsTweetLikedAsync(_authorizationService.CurrentUserId, Tweet.Id, Tweet.IsUserLiked);
        }

        private async Task MarkTappedHandler()
        {
            Tweet.IsUserMarked = !Tweet.IsUserMarked;
            await _tweetService.SetIsTweetMarkedAsync(_authorizationService.CurrentUserId, Tweet.Id, Tweet.IsUserMarked);
        }

        #endregion
    }
}
