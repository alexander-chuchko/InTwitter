using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Services.SearchService;
using InTwitter.Services.TweetService;
using InTwitter.Services.UserService;
using InTwitter.Models.User;
using InTwitter.Models;
using InTwitter.Models.Tweet;
using InTwitter.Models.Base;
using InTwitter.Enums;
using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.Views;

namespace InTwitter.ViewModels
{
    public class SearchTabPageViewModel : ViewModelBase
    {
        private IAuthorizationService _authorizationService;

        private IUserService _userService;

        private ISearchService _searchService;

        private IFeedService _feedService;

        private ITweetService _tweetService;

        private DateTime _lowerDateTime;

        private bool _isLoaded = false;

        public SearchTabPageViewModel(
                                      INavigationService navigationService,
                                      IAuthorizationService authorizationService,
                                      IUserService userService,
                                      ISearchService searchService,
                                      IFeedService feedService,
                                      ITweetService tweetService)
            : base(navigationService)
        {
            this._authorizationService = authorizationService;
            this._userService = userService;
            this._searchService = searchService;
            this._feedService = feedService;
            this._tweetService = tweetService;

            SearchBarState = ESearchBarStates.Icon;
            PopularThemes = new ObservableCollection<PopularTheme>();
            Tweets = new ObservableCollection<TweetViewModel>();

            _lowerDateTime = DateTime.Now;

            Initialize();
        }

        #region ---Public Properties---

        public ObservableCollection<TweetViewModel> Tweets { get; set; }

        private ObservableCollection<PopularTheme> _popularThemes;
        public ObservableCollection<PopularTheme> PopularThemes
        {
            get => _popularThemes;
            set => SetProperty(ref _popularThemes, value);
        }

        private ESearchBarStates _searchBarStates;
        public ESearchBarStates SearchBarState
        {
            get => _searchBarStates;
            set => SetProperty(ref _searchBarStates, value);
        }

        private UserViewModel _user;
        public UserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private string _searchField;
        public string SearchField
        {
            get => _searchField;
            set => SetProperty(ref _searchField, value);
        }

        private ESearchPageStatus _pageStatus;
        public ESearchPageStatus PageStatus
        {
            get => _pageStatus;
            set => SetProperty(ref _pageStatus, value);
        }

        public string NoResultMessage => $"\"{_searchField}\"";

        public ICommand FinishedScrollCommand => SingleExecutionCommand.FromFunc(FinishedScrollHandler);

        public ICommand TweetTapped => SingleExecutionCommand.FromFunc<TweetViewModel>(TweetTappedHandler);

        public ICommand UserTapped => SingleExecutionCommand.FromFunc<UserViewModel>(UserTappedHandler);

        public ICommand MediaTapped => SingleExecutionCommand.FromFunc<MediaSourceViewModel>(MediaTappedHandler);

        public ICommand PopularThemeTapped => SingleExecutionCommand.FromFunc<PopularTheme>(PopularThemeTappedHandler);

        public ICommand ClearTappedCommand => SingleExecutionCommand.FromFunc<PopularTheme>(ClearTappedHandler);

        public ICommand LikeTappedCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(LikeTappedHandler);

        public ICommand MarkTappedCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(MarkTappedHandler);

        public ICommand SearchCompleteCommand => SingleExecutionCommand.FromFunc(SearchCompleteHandler);

        #endregion

        #region ---Overrides---

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("hashtag"))
            {
                SearchField = (string)parameters["hashtag"];
                ReloadTweets();
            }

            if (Tweets.Count != 0)
            {
                var result = await _tweetService.GetUpToDateTweets(Tweets.Select(t => t as IVersionController).ToList());

                if (result.IsSuccess && result.Result.Count != 0)
                {
                    foreach (var tweet in result.Result)
                    {
                        var tweetVM = tweet.GetViewModel(_authorizationService.CurrentUserId);
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(SearchField):

                    if (string.IsNullOrEmpty(SearchField))
                    {
                        PageStatus = ESearchPageStatus.PopularThemes;
                    }

                    break;
            }
        }

        #endregion

        #region ---Private Helpers---

        private async Task AddTweets(List<Tweet> tweets, TimeDirection timeDirection)
        {
            _isLoaded = true;

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

                        Tweets.Add(tweet.GetViewModel(_authorizationService.CurrentUserId));
                        await Task.Delay(32);
                    }
                }
            }

            _isLoaded = false;
        }

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

        private Task SearchCompleteHandler()
        {
            if (!string.IsNullOrEmpty(SearchField))
            {
                ReloadTweets();
            }

            return Task.CompletedTask;
        }

        private Task ClearTappedHandler(PopularTheme arg)
        {
            SearchField = string.Empty;

            return Task.CompletedTask;
        }

        private Task PopularThemeTappedHandler(PopularTheme theme)
        {
            SearchField = theme.Theme;

            ReloadTweets();

            return Task.CompletedTask;
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

        private async Task TweetTappedHandler(TweetViewModel tweet)
        {
            await NavigationService.NavigateAsync(nameof(TweetPage), new NavigationParameters()
            {
                { nameof(TweetViewModel), tweet },
            });
        }

        private async Task UserTappedHandler(UserViewModel user)
        {
            var parameters = new NavigationParameters();
            if (user != null)
            {
                parameters.Add(nameof(User), user.Id);
            }
            else
            {
                parameters.Add(nameof(User), _authorizationService.CurrentUserId);
            }

            await NavigationService.NavigateAsync(nameof(ProfilePage), parameters);
        }

        private async Task FinishedScrollHandler()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                var result = await _feedService.GetTweetsBySearch(SearchField, _lowerDateTime, TimeDirection.Earlier);

                if (result.IsSuccess)
                {
                    await AddTweets(result.Result, TimeDirection.Earlier);
                }

                _isLoaded = false;
            }
        }

        private async void Initialize()
        {
            var aOResult = await _userService.GetUserAsync(_authorizationService.CurrentUserId);

            if (aOResult.IsSuccess)
            {
                User = aOResult.Result.ToUserViewModel();
            }

            var result = await _searchService.GetPopularThemeAsync();

            if (result.IsSuccess)
            {
                for (int i = 0; i < 5 && i < result.Result.Count; i++)
                {
                    PopularThemes.Add(result.Result[i]);
                }
            }

            PageStatus = ESearchPageStatus.PopularThemes;
        }

        private async void ReloadTweets()
        {
            PageStatus = ESearchPageStatus.LoadingResults;

            await Task.Delay(250);

            Tweets.Clear();
            _lowerDateTime = DateTime.Now;

            var result = await _feedService.GetTweetsBySearch(SearchField, _lowerDateTime, TimeDirection.Earlier);

            if (result.IsSuccess)
            {
                await AddTweets(result.Result, TimeDirection.Earlier);
            }

            if (Tweets.Count == 0 && !string.IsNullOrEmpty(SearchField))
            {
                PageStatus = ESearchPageStatus.EmptyResult;
            }
            else
            {
                PageStatus = ESearchPageStatus.ShowResults;
            }
        }

        #endregion
    }
}
