using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using InTwitter.Models;
using InTwitter.Models.Tweet;
using InTwitter.Enums;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.TweetService;
using InTwitter.Helpers;
using InTwitter.Views;
using InTwitter.Services.FeedService;

namespace InTwitter.ViewModels
{
    public class TweetPageViewModel : ViewModelBase
    {
        private IAuthorizationService _authorizationService;

        private ITweetService _tweetService;

        public TweetPageViewModel(
                                  INavigationService navigationService,
                                  IAuthorizationService authorizationService,
                                  ITweetService tweetService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _tweetService = tweetService;
        }

        #region ---Public Properties---

        private TweetViewModel _tweet;
        public TweetViewModel Tweet
        {
            get => _tweet;
            set => SetProperty(ref _tweet, value);
        }

        private bool _isLoaded = false;
        public bool IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }

        public ICommand LikeTappedCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(LikeTappedHandler);

        public ICommand MarkTappedCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(MarkTappedHandler);

        public ICommand MediaTappedCommand => SingleExecutionCommand.FromFunc<MediaSourceViewModel>(MediaTappedHandler);

        #endregion

        #region ---Overrides---

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            RaisePropertyChanged(nameof(IsLoaded));

            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(nameof(TweetViewModel)))
            {
                Tweet = parameters[nameof(TweetViewModel)] as TweetViewModel;
            }

            IsLoaded = true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            MediaCenter.StopAllPlayers();
        }

        #endregion

        #region ---Private Helpers---

        private async Task LikeTappedHandler(TweetViewModel tweet)
        {
            tweet.IsUserLiked = !tweet.IsUserLiked;
            tweet.LikesAmount += tweet.IsUserLiked ? +1 : -1;

            await _tweetService.SetIsTweetLikedAsync(_authorizationService.CurrentUserId, tweet.Id, tweet.IsUserLiked);
        }

        private async Task MarkTappedHandler(TweetViewModel tweet)
        {
            tweet.IsUserMarked = !tweet.IsUserMarked;
            await _tweetService.SetIsTweetMarkedAsync(_authorizationService.CurrentUserId, tweet.Id, tweet.IsUserMarked);
        }

        private async Task MediaTappedHandler(MediaSourceViewModel mediaSource)
        {
            switch (mediaSource.MediaType)
            {
                case EMediaType.Image:
                    var index = Tweet.MediaSources.IndexOf(mediaSource);

                    await NavigationService.NavigateAsync(nameof(TweetMediaPage), new NavigationParameters()
                        {
                            { nameof(TweetViewModel), Tweet },
                            { "position", index },
                        });
                    break;
                case EMediaType.Gif:
                    mediaSource.IsPlay = !mediaSource.IsPlay;
                    break;
                case EMediaType.Video:
                    break;
            }
        }

        #endregion
    }
}
