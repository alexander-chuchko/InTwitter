using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using InTwitter.Models;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using Prism.Navigation;
using Xamarin.Forms;
using InTwitter.Extensions;
using System.Linq;
using InTwitter.Views;
using InTwitter.Enums;
using InTwitter.Models.User;
using InTwitter.Helpers;
using InTwitter.Services.TweetService;
using InTwitter.Models.Tweet;
using InTwitter.Models.Base;
using InTwitter.Services.UserService;
using InTwitter.Models.Icon;
using System.ComponentModel;
using InTwitter.Services.MediaService;
using InTwitter.Services.StoriesService;

namespace InTwitter.ViewModels
{
    public abstract class BaseFeedPageViewModel : ViewModelBase, IInitializeAsync
    {
        private DateTime _upperDateTime;

        protected Guid _currentUserId;

        protected DateTime _lowerDateTime;

        private bool _isLoaded = false;

        protected UserViewModel _currentUser;

        protected IAuthorizationService AuthorizationService { get; set; }

        protected IUserService UserService { get; set; }

        protected IFeedService FeedService { get; set; }

        protected ITweetService TweetService { get; set; }

        public BaseFeedPageViewModel(
                                     INavigationService navigationService,
                                     IAuthorizationService authorizationService,
                                     IFeedService feedService,
                                     ITweetService tweetService,
                                     IUserService userService)
            : base(navigationService)
        {
            this.Tweets = new ObservableCollection<TweetViewModel>();

            this.AuthorizationService = authorizationService;
            this.FeedService = feedService;
            this.UserService = userService;
            this.TweetService = tweetService;

            _currentUserId = this.AuthorizationService.CurrentUserId;

            _upperDateTime = DateTime.Now;
            _lowerDateTime = DateTime.Now;
        }

        #region ---Public Properties---

        public ObservableCollection<TweetViewModel> Tweets { get; set; }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private EFeedViewState _feedState;

        public EFeedViewState FeedState
        {
            get => _feedState;
            set => SetProperty(ref _feedState, value);
        }

        private double _scrollPosition;

        public double ScrollPosition
        {
            get => _scrollPosition;
            set => SetProperty(ref _scrollPosition, value);
        }

        private string _currentProfileIcon;
        public string CurrentProfileIcon
        {
            get => _currentProfileIcon;
            set => SetProperty(ref _currentProfileIcon, value);
        }

        public ICommand AddPostCommand => SingleExecutionCommand.FromFunc(OnAddPostCommandAsync);

        public ICommand RefreshCommand => SingleExecutionCommand.FromFunc(RefreshCommandHandler);

        public ICommand FinishedScrollCommand => SingleExecutionCommand.FromFunc(FinishedScrollHandler);

        public ICommand TweetTapped => SingleExecutionCommand.FromFunc<TweetViewModel>(TweetTappedHandler);

        public ICommand MoreTextTapped => SingleExecutionCommand.FromFunc<TweetViewModel>(MoreTextTappedHandler);

        public ICommand HashtagTapped => SingleExecutionCommand.FromFunc<string>(HashtagTappedHandler);

        public ICommand LikeTappedCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(LikeTappedHandler);

        public ICommand MarkTappedCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(MarkTappedHandler);

        public ICommand UserTapped => SingleExecutionCommand.FromFunc<UserViewModel>(UserTapHandler);

        public ICommand MediaTapped => SingleExecutionCommand.FromFunc<MediaSourceViewModel>(MediaTappedHandler);
        #endregion

        #region ---IInitializeAsync Implementation---

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            AddTweets(await GetPartOfTweets(_currentUserId, DateTime.Now, TimeDirection.Earlier), TimeDirection.Earlier);

            var user = await UserService.GetUserAsync(_currentUserId);

            if (user.IsSuccess)
            {
                _currentUser = user.Result.ToUserViewModel();

                if (!string.IsNullOrEmpty(_currentUser.IconSource))
                {
                    CurrentProfileIcon = _currentUser.IconSource;
                }
                else
                {
                    CurrentProfileIcon = "pic_profile_small.png";
                }
            }
        }

        #endregion

        #region ---Overrides---

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (Tweets.Count != 0)
            {
                var result = await TweetService.GetUpToDateTweets(Tweets.Select(t => t as IVersionController).ToList());

                if (result.IsSuccess && result.Result.Count != 0)
                {
                    foreach (var tweet in result.Result)
                    {
                        var tweetVM = tweet.GetViewModel(_currentUserId);
                        var index = Tweets.IndexOf(tweetVM);

                        if (index != -1)
                        {
                            Tweets[index].IsUserLiked = tweetVM.IsUserLiked;
                            Tweets[index].LikesAmount = tweetVM.LikesAmount;
                            Tweets[index].IsUserMarked = tweetVM.IsUserMarked;
                        }
                    }
                }
            }
        }

        #endregion

        #region ---Protected virtual methods---

        protected abstract Task<List<Tweet>> GetPartOfTweets(Guid userId, DateTime timePoint, TimeDirection direction);

        protected virtual async Task UserTapHandler(UserViewModel user)
        {
            var parameters = new NavigationParameters();
            if (user != null)
            {
                parameters.Add(nameof(User), user.Id);
            }
            else
            {
                parameters.Add(nameof(User), AuthorizationService.CurrentUserId);
            }

            await NavigationService.NavigateAsync(nameof(ProfilePage), parameters);
        }

        protected virtual async Task TweetTappedHandler(TweetViewModel tweet)
        {
            await NavigationService.NavigateAsync(nameof(TweetPage), new NavigationParameters()
            {
                { nameof(TweetViewModel), tweet },
            });
        }

        protected virtual async Task FinishedScrollHandler()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                FeedState = EFeedViewState.FeedLoading;

                await Task.Delay(100);

                var result = await GetPartOfTweets(_currentUserId, _lowerDateTime, TimeDirection.Earlier);

                if (result.Count == 0)
                {
                    FeedState = EFeedViewState.NoResults;
                }
                else
                {
                    await AddTweets(result, TimeDirection.Earlier);
                    await Task.Delay(100);
                }

                _isLoaded = false;
            }
        }

        protected virtual async Task RefreshCommandHandler()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                IsRefreshing = true;

                var tweets = await GetPartOfTweets(_currentUserId, _upperDateTime, TimeDirection.Later);
                if (tweets.Count != 0)
                {
                    await AddTweets(tweets, TimeDirection.Later);
                }
                else
                {
                    DependencyService.Get<ISnackbarShower>().Show(TextResources["UnableUpdateFeedMessage"]);
                }

                IsRefreshing = false;
                _isLoaded = false;
            }
            else
            {
                IsRefreshing = false;
            }
        }

        protected virtual async Task AddTweets(List<Tweet> tweets, TimeDirection timeDirection)
        {
            if (tweets.Count != 0)
            {
                if (timeDirection == TimeDirection.Earlier)
                {
                    foreach (var tweet in tweets)
                    {
                        if (_lowerDateTime > tweet.CreationTime)
                        {
                            _lowerDateTime = tweet.CreationTime;
                        }

                        Tweets.Add(tweet.GetViewModel(_currentUserId));
                        await Task.Delay(10);
                    }
                }

                if (timeDirection == TimeDirection.Later)
                {
                    foreach (var tweet in tweets)
                    {
                        if (_upperDateTime < tweet.CreationTime)
                        {
                            _upperDateTime = tweet.CreationTime;
                        }

                        Tweets.Insert(0, tweet.GetViewModel(_currentUserId));
                    }
                }
            }
        }

        protected virtual async Task MarkTappedHandler(TweetViewModel tweet)
        {
            tweet.IsUserMarked = !tweet.IsUserMarked;
            await TweetService.SetIsTweetMarkedAsync(_currentUserId, tweet.Id, tweet.IsUserMarked);
        }

        #endregion

        #region ---Private Helpers---

        private async Task LikeTappedHandler(TweetViewModel tweet)
        {
            tweet.IsUserLiked = !tweet.IsUserLiked;
            tweet.LikesAmount += tweet.IsUserLiked ? +1 : -1;

            await TweetService.SetIsTweetLikedAsync(_currentUserId, tweet.Id, tweet.IsUserLiked);
        }

        private async Task HashtagTappedHandler(string hashtag)
        {
            await NavigationService.SelectTabAsync(nameof(SearchTabPage), new NavigationParameters() { { nameof(hashtag), hashtag } });
        }

        private Task MoreTextTappedHandler(TweetViewModel tweet)
        {
            if (tweet.IsShortText == true && tweet.IsTextShorted == true)
            {
                tweet.IsShortText = false;
            }

            return Task.CompletedTask;
        }

        private Task OnAddPostCommandAsync()
        {
            return NavigationService.NavigateAsync(nameof(AddPostPage), null, true, true);
        }

        private async Task MediaTappedHandler(MediaSourceViewModel mediaSource)
        {
            var tweet = Tweets.Where(t => t.Id == mediaSource.TweetId).FirstOrDefault();

            if (tweet != null)
            {
                switch (mediaSource.MediaType)
                {
                    case EMediaType.Image:
                        var index = tweet.MediaSources.IndexOf(mediaSource);

                        await NavigationService.NavigateAsync(nameof(TweetMediaPage), new NavigationParameters()
                        {
                            { nameof(TweetViewModel), tweet },
                            { "position", index },
                        });
                        break;
                    case EMediaType.Gif:
                        mediaSource.IsPlay = !mediaSource.IsPlay;
                        break;
                    case EMediaType.Video:
                        await NavigationService.NavigateAsync(nameof(TweetPage), new NavigationParameters()
                        {
                            { nameof(TweetViewModel), tweet },
                        });
                        break;
                }
            }
        }

        #endregion
    }
}