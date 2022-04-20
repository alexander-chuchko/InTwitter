using InTwitter.Models;
using InTwitter.Models.Tweet;
using InTwitter.Models.User;
using InTwitter.Enums;
using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Services.TweetService;
using InTwitter.Services.UserInteractionService;
using InTwitter.Services.UserService;
using InTwitter.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using InTwitter.Models.Base;
using InTwitter.Services.StoriesService;
using InTwitter.Models.Stories;
using InTwitter.Models.Icon;
using InTwitter.Services.PermissionService;

namespace InTwitter.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IFeedService _feedService;
        private readonly ITweetService _tweetService;
        private readonly IUserInteractionService _userInteractionService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IImageSaver _imageSaver;
        private readonly IStoriesService _storiesService;
        private readonly IPermissionService _permissionService;

        private Guid _userId;
        private DateTime _upperPostsDateTime;
        private DateTime _upperLikesDateTime;
        private DateTime _lowerPostsDateTime;
        private DateTime _lowerLikesDateTime;
        private bool _isLoaded;

        public ProfilePageViewModel(
            INavigationService navigationService,
            IAuthorizationService authorizationService,
            IUserService userService,
            IFeedService feedService,
            ITweetService tweetService,
            IUserInteractionService userInteractionService,
            IPageDialogService pageDialogService,
            IStoriesService storiesService,
            IPermissionService permissionService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _userService = userService;
            _feedService = feedService;
            _tweetService = tweetService;
            _userInteractionService = userInteractionService;
            _pageDialogService = pageDialogService;
            _storiesService = storiesService;
            _permissionService = permissionService;
            _imageSaver = DependencyService.Get<IImageSaver>();
            _isLoaded = false;

            _upperPostsDateTime = DateTime.Now;
            _upperLikesDateTime = DateTime.Now;
            _lowerPostsDateTime = DateTime.Now;
            _lowerLikesDateTime = DateTime.Now;

            IsWallpaperLoaderVisible = true;
            IsIconLoaderVisible = true;

            Icon ??= "pic_profile_small.png";

            Posts = new ObservableCollection<TweetViewModel>();
            Likes = new ObservableCollection<TweetViewModel>();

            MuteButtonText = TextResources["Mute"];
            IsMuteButtonEnabled = true;
            BlacklistButtonText = TextResources["AddToBlackList"];
            IsBlacklistButtonEnabled = true;

            IsPostsVisible = true;
        }

        #region ---Public properties---

        private bool _isCurrentUserDetailVisible;
        public bool IsCurrentUserDetailVisible
        {
            get => _isCurrentUserDetailVisible;
            set => SetProperty(ref _isCurrentUserDetailVisible, value);
        }

        private bool _isAnotherUserDetailVisible;
        public bool IsAnotherUserDetailVisible
        {
            get => _isAnotherUserDetailVisible;
            set => SetProperty(ref _isAnotherUserDetailVisible, value);
        }

        private bool _isMuteButtonEnabled;
        public bool IsMuteButtonEnabled
        {
            get => _isMuteButtonEnabled;
            set => SetProperty(ref _isMuteButtonEnabled, value);
        }

        private string _muteButtonText;
        public string MuteButtonText
        {
            get => _muteButtonText;
            set => SetProperty(ref _muteButtonText, value);
        }

        private bool _isMuteMessageVisible;
        public bool IsMuteMessageVisible
        {
            get => _isMuteMessageVisible;
            set => SetProperty(ref _isMuteMessageVisible, value);
        }

        private string _muteMessageText;
        public string MuteMessageText
        {
            get => _muteMessageText;
            set => SetProperty(ref _muteMessageText, value);
        }

        private bool _isBlacklistButtonEnabled;
        public bool IsBlacklistButtonEnabled
        {
            get => _isBlacklistButtonEnabled;
            set => SetProperty(ref _isBlacklistButtonEnabled, value);
        }

        private string _blacklistButtonText;
        public string BlacklistButtonText
        {
            get => _blacklistButtonText;
            set => SetProperty(ref _blacklistButtonText, value);
        }

        private bool _isWallpaperVisible;
        public bool IsWallpaperVisible
        {
            get => _isWallpaperVisible;
            set => SetProperty(ref _isWallpaperVisible, value);
        }

        private bool _isWallpaperLoaderVisible;
        public bool IsWallpaperLoaderVisible
        {
            get => _isWallpaperLoaderVisible;
            set => SetProperty(ref _isWallpaperLoaderVisible, value);
        }

        private string _wallPaper;
        public string WallPaper
        {
            get => _wallPaper;
            set => SetProperty(ref _wallPaper, value);
        }

        private bool _isIconLoaderVisible;
        public bool IsIconLoaderVisible
        {
            get => _isIconLoaderVisible;
            set => SetProperty(ref _isIconLoaderVisible, value);
        }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private bool _isChangeProfileButtonVisible;
        public bool IsChangeProfileButtonVisible
        {
            get => _isChangeProfileButtonVisible;
            set => SetProperty(ref _isChangeProfileButtonVisible, value);
        }

        private bool _isProfileInBlacklistVisible;
        public bool IsProfileInBlacklistVisible
        {
            get => _isProfileInBlacklistVisible;
            set => SetProperty(ref _isProfileInBlacklistVisible, value);
        }

        private bool _isMutedVisible;
        public bool IsMutedVisible
        {
            get => _isMutedVisible;
            set => SetProperty(ref _isMutedVisible, value);
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

        private bool _isOutsideTappingEnabled;
        public bool IsOutsideTappingEnabled
        {
            get => _isOutsideTappingEnabled;
            set => SetProperty(ref _isOutsideTappingEnabled, value);
        }

        private Color _ColorStories = Color.FromHex("#DEDFE1");
        public Color ColorStories
        {
            get => _ColorStories;
            set => SetProperty(ref _ColorStories, value);
        }

        private bool _IsVisableIconPlus;
        public bool IsVisableIconPlus
        {
            get => _IsVisableIconPlus;
            set => SetProperty(ref _IsVisableIconPlus, value);
        }

        public ObservableCollection<TweetViewModel> Posts { get; set; }

        public ObservableCollection<TweetViewModel> Likes { get; set; }

        private EProfileTweetsState _selectedTab;
        public EProfileTweetsState SelectedTab
        {
            get => _selectedTab;
            set => SetProperty(ref _selectedTab, value);
        }

        private bool _isPostsVisible;
        public bool IsPostsVisible
        {
            get => _isPostsVisible;
            set => SetProperty(ref _isPostsVisible, value);
        }

        private bool _isLikesVisible;
        public bool IsLikesVisible
        {
            get => _isLikesVisible;
            set => SetProperty(ref _isLikesVisible, value);
        }

        private UserStoryViewModel _UserStoryVM;
        public UserStoryViewModel UserStoryVM
        {
            get => _UserStoryVM;
            set => SetProperty(ref _UserStoryVM, value);
        }

        public ICommand MutedProfilesTapCommand => SingleExecutionCommand.FromFunc(OnMutedProfilesTapCommand);

        public ICommand OutsideTapCommand => SingleExecutionCommand.FromFunc(OnOutsideTap);

        public ICommand GoBackTapCommand => SingleExecutionCommand.FromFunc(OnGoBackTap);

        public ICommand OpenDetailTapCommand => SingleExecutionCommand.FromFunc(OnOpenDetailTap);

        public ICommand LoadedWallpaperCommand => SingleExecutionCommand.FromFunc(OnLoadedWallpaper);

        public ICommand LoadedIconCommand => SingleExecutionCommand.FromFunc(OnLoadedIcon);

        public ICommand ShareTapCommand => SingleExecutionCommand.FromFunc(OnShareTap);

        public ICommand ChangeProfileTapCommand => SingleExecutionCommand.FromFunc(OnChangeProfileTap);

        public ICommand OpenBlacklistTapCommand => SingleExecutionCommand.FromFunc(OnOpenBlacklistTap);

        public ICommand MuteTapCommand => SingleExecutionCommand.FromFunc(OnMuteTap);

        public ICommand BlacklistTapCommand => SingleExecutionCommand.FromFunc(OnBlacklistTap);

        public ICommand SelectPostsTapCommand => SingleExecutionCommand.FromFunc(OnSelectPostsTap);

        public ICommand SelectLikesTapCommand => SingleExecutionCommand.FromFunc(OnSelectLikesTap);

        public ICommand TweetTapCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnTweetTap);

        public ICommand PostsMediaTapCommand => SingleExecutionCommand.FromFunc<MediaSourceViewModel>(OnPostsMediaTap);

        public ICommand LikesMediaTapCommand => SingleExecutionCommand.FromFunc<MediaSourceViewModel>(OnLikesMediaTap);

        public ICommand MoreTextTapCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnMoreTextTap);

        public ICommand UserTapCommand => SingleExecutionCommand.FromFunc<UserViewModel>(OnUserTap);

        public ICommand PostsLikeTapCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnPostsLikeTap);

        public ICommand LikesLikeTapCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnLikesLikeTap);

        public ICommand PostsMarkTapCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnPostsMarkTap);

        public ICommand LikesMarkTapCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnLikesMarkTap);

        public ICommand FinishedPostsScrollCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnFinishedPostsScroll);

        public ICommand FinishedLikesScrollCommand => SingleExecutionCommand.FromFunc<TweetViewModel>(OnFinishedLikesScroll);

        public ICommand TapUserStroryCommand => SingleExecutionCommand.FromFunc(OnTapStoriesAsync);

        #endregion

        #region ---Overrides---

        private async Task CreateBorderStories()
        {
            List<PostData> postDatas = null;
            var getStoriesResult = await _storiesService.GetByIdStoriesModelAsync<UserStories>(_userId.ToString());

            if (getStoriesResult != null)
            {
                if (_userId.Equals(_authorizationService.CurrentUserId))
                {
                    UserStoryVM = getStoriesResult.ToUserStoryViewModelFromUserStories(postDatas);
                    IsVisableIconPlus = !UserStoryVM.IsLoaded;
                    ColorStories = UserStoryVM.OutlineСolor;
                }
                else
                {
                    if (getStoriesResult.MediaStoriesSources.IsAny())
                    {
                        postDatas = await _storiesService.GetIdsFollowers<UserStories>();
                        UserStoryVM = getStoriesResult.ToUserStoryViewModelFromUserStories(postDatas);
                        ColorStories = UserStoryVM.OutlineСolor;
                        IsVisableIconPlus = UserStoryVM.IsAuthorized;
                    }
                    else
                    {
                        ColorStories = Color.Transparent;
                        IsVisableIconPlus = false;
                    }
                }
            }
            else
            {
                ColorStories = Color.Transparent;
                IsVisableIconPlus = _userId.Equals(_authorizationService.CurrentUserId)
                    ? true
                    : false;
            }
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            if (!parameters.TryGetValue(nameof(User), out _userId))
            {
                _userId = _authorizationService.CurrentUserId;
            }

            var user = (await _userService.GetUserAsync(_userId)).Result;
            var profile = user.ToUserViewModel();
            WallPaper = profile.WallPapperSource;
            Icon = profile.IconSource;
            Name = profile.Name;
            Email = profile.Email;

            await CreateBorderStories();

            if (WallPaper == null)
            {
                IsWallpaperLoaderVisible = false;
            }

            if (_userId.Equals(_authorizationService.CurrentUserId))
            {
                IsChangeProfileButtonVisible = true;
            }
            else
            {
                var isUserMuted = await _userInteractionService.GetIsUserMuted(_authorizationService.CurrentUserId, _userId);
                var isUserInBlacklist = await _userInteractionService.GetIsUserInBlacklist(_authorizationService.CurrentUserId, _userId);
                if (isUserMuted.IsSuccess)
                {
                    IsMutedVisible = true;
                    MuteButtonText = TextResources["Unmute"];
                    IsBlacklistButtonEnabled = false;
                }
                else if (isUserInBlacklist.IsSuccess)
                {
                    IsProfileInBlacklistVisible = true;
                    BlacklistButtonText = TextResources["RemoveFromBlacklist"];
                    IsMuteButtonEnabled = false;
                }
            }

            var partOfTweetsFromUser = await GetPartOfTweetsFromUser(_userId, DateTime.Now, TimeDirection.Earlier);
            AddTweetsFromUser(partOfTweetsFromUser, TimeDirection.Earlier);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (Posts.Count != 0)
            {
                var result = await _tweetService.GetUpToDateTweets(Posts.Select(t => t as IVersionController).ToList());

                if (result.IsSuccess && result.Result.Count != 0)
                {
                    foreach (var tweet in result.Result)
                    {
                        var tweetVM = tweet.GetViewModel(_userId);
                        var index = Posts.IndexOf(tweetVM);

                        if (index != -1)
                        {
                            Posts[index].IsUserMarked = tweetVM.IsUserMarked;
                        }
                    }
                }
            }

            if (Likes.Count == 0)
            {
                var partOfTweetsLikedUser = await GetPartOfTweetsLikedUser(_userId, DateTime.Now, TimeDirection.Earlier);
                AddTweetsLikedUser(partOfTweetsLikedUser, TimeDirection.Earlier);
            }
            else
            {
                var result = await _tweetService.GetUpToDateTweets(Likes.Select(t => t as IVersionController).ToList());

                if (result.IsSuccess && result.Result.Count != 0)
                {
                    foreach (var tweet in result.Result)
                    {
                        var tweetVM = tweet.GetViewModel(_userId);
                        var index = Likes.IndexOf(tweetVM);

                        if (index != -1)
                        {
                            Likes[index].IsUserMarked = tweetVM.IsUserMarked;
                        }
                    }
                }

                List<Tweet> tweetslist = null;
                AOResult<List<Tweet>> getList;

                getList = await _feedService.GetTweetsLikedUser(_userId, DateTime.Now, TimeDirection.Earlier, Likes.Count);

                if (getList.IsSuccess)
                {
                    tweetslist = getList.Result;

                    if (tweetslist != null)
                    {
                        await UpdateTweets(tweetslist);
                    }
                }
            }
        }

        #endregion

        #region ---Private helpers---

        protected async Task<List<Tweet>> GetPartOfTweetsFromUser(Guid userId, DateTime timePoint, TimeDirection direction)
        {
            var result = await _feedService.GetTweetsFromUser(userId, timePoint, direction);
            return result.Result;
        }

        protected async Task<List<Tweet>> GetPartOfTweetsLikedUser(Guid userId, DateTime timePoint, TimeDirection direction)
        {
            var result = await _feedService.GetTweetsLikedUser(userId, timePoint, direction);
            return result.Result;
        }

        protected async Task AddTweetsFromUser(List<Tweet> tweets, TimeDirection timeDirection)
        {
            _isLoaded = true;
            if (tweets.Count != 0)
            {
                if (timeDirection == TimeDirection.Earlier)
                {
                    foreach (var tweet in tweets)
                    {
                        if (_lowerPostsDateTime > tweet.CreationTime)
                        {
                            _lowerPostsDateTime = tweet.CreationTime;
                        }

                        Posts.Add(tweet.GetViewModel(_authorizationService.CurrentUserId));
                        await Task.Delay(32);
                    }
                }

                if (timeDirection == TimeDirection.Later)
                {
                    foreach (var tweet in tweets)
                    {
                        if (_upperPostsDateTime < tweet.CreationTime)
                        {
                            _upperPostsDateTime = tweet.CreationTime;
                        }

                        Posts.Insert(0, tweet.GetViewModel(_authorizationService.CurrentUserId));
                    }
                }
            }

            _isLoaded = false;
        }

        protected async Task AddTweetsLikedUser(List<Tweet> tweets, TimeDirection timeDirection)
        {
            _isLoaded = true;
            if (tweets.Count != 0)
            {
                if (timeDirection == TimeDirection.Earlier)
                {
                    foreach (var tweet in tweets)
                    {
                        if (_lowerLikesDateTime > tweet.CreationTime)
                        {
                            _lowerLikesDateTime = tweet.CreationTime;
                        }

                        Likes.Add(tweet.GetViewModel(_authorizationService.CurrentUserId));
                        await Task.Delay(32);
                    }
                }

                if (timeDirection == TimeDirection.Later)
                {
                    foreach (var tweet in tweets)
                    {
                        if (_upperLikesDateTime < tweet.CreationTime)
                        {
                            _upperLikesDateTime = tweet.CreationTime;
                        }

                        Likes.Insert(0, tweet.GetViewModel(_authorizationService.CurrentUserId));
                    }
                }
            }

            _isLoaded = false;
        }

        private async Task UpdateTweets(List<Tweet> list)
        {
            for (int i = 0; i < Likes.Count; i++)
            {
                Tweet tweet = list.FirstOrDefault(t => t.Id == Likes[i].Id);

                if (tweet != null)
                {
                    list.Remove(tweet);
                }
                else
                {
                    Likes.RemoveAt(i--);
                }
            }

            int counter = 0;

            for (int i = 0; i < Likes.Count; i++)
            {
                if (counter > list.Count - 1)
                {
                    break;
                }

                if (Likes[i].CreationTime <= list[counter].CreationTime)
                {
                    Likes.Insert(i, list[counter].GetViewModel(_authorizationService.CurrentUserId));
                    counter++;
                }
            }

            if (counter < list.Count)
            {
                for (int i = counter; i < list.Count; i++)
                {
                    Likes.Add(list[i].GetViewModel(_authorizationService.CurrentUserId));
                }
            }

            if (Likes.Count > 0)
            {
                _lowerLikesDateTime = Likes[Likes.Count - 1].CreationTime;
            }
            else
            {
                _lowerLikesDateTime = DateTime.Now;
            }
        }

        private Task OnOutsideTap()
        {
            IsCurrentUserDetailVisible = false;
            IsAnotherUserDetailVisible = false;
            IsOutsideTappingEnabled = false;
            return Task.CompletedTask;
        }

        private async Task OnGoBackTap()
        {
            (Application.Current.MainPage as MainMasterDetailPage).IsGestureEnabled = true;
            await NavigationService.GoBackAsync();
        }

        private Task OnOpenDetailTap()
        {
            IsOutsideTappingEnabled = true;
            if (_userId.Equals(_authorizationService.CurrentUserId))
            {
                IsCurrentUserDetailVisible = true;
            }
            else
            {
                IsAnotherUserDetailVisible = true;
            }

            return Task.CompletedTask;
        }

        private Task OnLoadedWallpaper()
        {
            IsWallpaperLoaderVisible = false;
            return Task.CompletedTask;
        }

        private Task OnLoadedIcon()
        {
            IsIconLoaderVisible = false;
            return Task.CompletedTask;
        }

        private async Task OnShareTap()
        {
            IsOutsideTappingEnabled = false;
            IsCurrentUserDetailVisible = false;
            IsAnotherUserDetailVisible = false;
            var imageSource = await _imageSaver.Save(Icon, true);

            var file = new ShareFileRequest()
            {
                File = new ShareFile(imageSource),
            };

            await Share.RequestAsync(file);
        }

        private async Task OnChangeProfileTap()
        {
            IsOutsideTappingEnabled = false;
            IsCurrentUserDetailVisible = false;
            await NavigationService.NavigateAsync(nameof(EditProfilePage));
        }

        private async Task OnOpenBlacklistTap()
        {
            IsOutsideTappingEnabled = false;
            IsCurrentUserDetailVisible = false;
            await NavigationService.NavigateAsync(nameof(BlacklistPage));
        }

        private async Task OnMuteTap()
        {
            IsOutsideTappingEnabled = false;
            IsAnotherUserDetailVisible = false;

            if (MuteButtonText == TextResources["Mute"])
            {
                IsBlacklistButtonEnabled = false;
                await _userInteractionService.SetIsUserMuted(_authorizationService.CurrentUserId, _userId, false);
                MuteButtonText = TextResources["Unmute"];
                IsMutedVisible = true;

                MuteMessageText = TextResources["MuteMessage"];
                IsMuteMessageVisible = true;
                await Task.Delay(3000);
                IsMuteMessageVisible = false;
            }
            else
            {
                var isUnMute = await _pageDialogService.DisplayAlertAsync(
                    title: string.Format(TextResources["RemoveFromMuteMessage"], Name),
                    message: " ",
                    acceptButton: TextResources["Remove"],
                    cancelButton: TextResources["Cancel"]);

                if (isUnMute)
                {
                    IsBlacklistButtonEnabled = true;
                    await _userInteractionService.SetIsUserMuted(_authorizationService.CurrentUserId, _userId, true);
                    MuteButtonText = TextResources["Mute"];
                    IsMutedVisible = false;

                    MuteMessageText = TextResources["YouWillSeePostsFeedmessage"];
                    IsMuteMessageVisible = true;
                    await Task.Delay(3000);
                    IsMuteMessageVisible = false;
                }
            }
        }

        private async Task OnBlacklistTap()
        {
            IsOutsideTappingEnabled = false;
            IsAnotherUserDetailVisible = false;

            if (BlacklistButtonText == TextResources["AddToBlackList"])
            {
                var isAddToBlacklist = await _pageDialogService.DisplayAlertAsync(
                    title: string.Format(TextResources["AddNameToBlacklist"], Name),
                    message: TextResources["ThisUserWillNotSeeYourPost"],
                    acceptButton: TextResources["AddToBlackList"],
                    cancelButton: TextResources["Cancel"]);

                if (isAddToBlacklist)
                {
                    IsMuteButtonEnabled = false;
                    await _userInteractionService.SetIsUserInBlacklist(_authorizationService.CurrentUserId, _userId, false);
                    BlacklistButtonText = "Remove from blacklist";
                    IsProfileInBlacklistVisible = true;
                }
            }
            else
            {
                var isRemoveFromBlacklist = await _pageDialogService.DisplayAlertAsync(
                    title: string.Format(TextResources["RemoveNameFromBlackList"], Name),
                    message: " ",
                    acceptButton: TextResources["Remove"],
                    cancelButton: TextResources["Cancel"]);

                if (isRemoveFromBlacklist)
                {
                    IsMuteButtonEnabled = true;
                    await _userInteractionService.SetIsUserInBlacklist(_authorizationService.CurrentUserId, _userId, true);
                    BlacklistButtonText = TextResources["AddToBlackList"];
                    IsProfileInBlacklistVisible = false;
                }
            }
        }

        private Task OnSelectPostsTap()
        {
            if (!_isLoaded)
            {
                SelectedTab = EProfileTweetsState.Posts;
                IsLikesVisible = false;
                IsPostsVisible = true;
            }

            return Task.CompletedTask;
        }

        private Task OnSelectLikesTap()
        {
            if (!_isLoaded)
            {
                SelectedTab = EProfileTweetsState.Likes;
                IsPostsVisible = false;
                IsLikesVisible = true;
            }

            return Task.CompletedTask;
        }

        private async Task OnTweetTap(TweetViewModel tweet)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(TweetViewModel), tweet);

            await NavigationService.NavigateAsync(nameof(TweetPage), parameters);
        }

        private async Task OnPostsMediaTap(MediaSourceViewModel mediaSource)
        {
            var tweet = Posts.Where(t => t.Id == mediaSource.TweetId).FirstOrDefault();

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

        private async Task OnLikesMediaTap(MediaSourceViewModel mediaSource)
        {
            var tweet = Likes.Where(t => t.Id == mediaSource.TweetId).FirstOrDefault();

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

        private Task OnMoreTextTap(TweetViewModel tweet)
        {
            if (tweet.IsShortText == true && tweet.IsTextShorted == true)
            {
                tweet.IsShortText = false;
            }

            return Task.CompletedTask;
        }

        private async Task OnUserTap(UserViewModel user)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(User), user.Id);
            await NavigationService.NavigateAsync(nameof(ProfilePage), parameters);
        }

        private async Task OnPostsLikeTap(TweetViewModel tweet)
        {
            tweet.IsUserLiked = !tweet.IsUserLiked;
            tweet.LikesAmount += tweet.IsUserLiked ? +1 : -1;

            await _tweetService.SetIsTweetLikedAsync(_authorizationService.CurrentUserId, tweet.Id, tweet.IsUserLiked);

            var tweetIndex = Likes.IndexOf(tweet);

            if (_userId.Equals(_authorizationService.CurrentUserId))
            {
                if (tweetIndex > -1)
                {
                    Likes.RemoveAt(tweetIndex);
                }
                else
                {
                    if (Likes.Count == 0)
                    {
                        Likes.Add(tweet);
                    }
                    else
                    {
                        for (int i = 0; i < Likes.Count; i++)
                        {
                            if (Likes[i].CreationTime < tweet.CreationTime)
                            {
                                Likes.Insert(i, tweet);
                                break;
                            }
                        }

                        if (!Likes.Contains(tweet) && Likes.Count % 5 != 0)
                        {
                            Likes.Add(tweet);
                        }
                    }

                    _lowerLikesDateTime = Likes[Likes.Count - 1].CreationTime;
                }
            }
            else if (tweetIndex > -1)
            {
                Likes[tweetIndex].IsUserLiked = tweet.IsUserLiked;
                Likes[tweetIndex].LikesAmount = tweet.LikesAmount;
            }
        }

        private async Task OnLikesLikeTap(TweetViewModel tweet)
        {
            tweet.IsUserLiked = !tweet.IsUserLiked;
            tweet.LikesAmount += tweet.IsUserLiked ? +1 : -1;

            await _tweetService.SetIsTweetLikedAsync(_authorizationService.CurrentUserId, tweet.Id, tweet.IsUserLiked);

            if (_userId.Equals(_authorizationService.CurrentUserId))
            {
                Likes.Remove(tweet);
            }

            var tweetIndex = Posts.IndexOf(tweet);

            if (tweetIndex > -1)
            {
                Posts[tweetIndex].IsUserLiked = tweet.IsUserLiked;
                Posts[tweetIndex].LikesAmount = tweet.LikesAmount;
            }
        }

        private async Task OnPostsMarkTap(TweetViewModel tweet)
        {
            tweet.IsUserMarked = !tweet.IsUserMarked;

            await _tweetService.SetIsTweetMarkedAsync(_authorizationService.CurrentUserId, tweet.Id, tweet.IsUserMarked);

            var tweetIndex = Likes.IndexOf(tweet);

            if (tweetIndex > -1)
            {
                Likes[tweetIndex].IsUserMarked = tweet.IsUserMarked;
            }
        }

        private async Task OnLikesMarkTap(TweetViewModel tweet)
        {
            tweet.IsUserMarked = !tweet.IsUserMarked;

            await _tweetService.SetIsTweetMarkedAsync(_authorizationService.CurrentUserId, tweet.Id, tweet.IsUserMarked);

            var tweetIndex = Posts.IndexOf(tweet);

            if (tweetIndex > -1)
            {
                Posts[tweetIndex].IsUserMarked = tweet.IsUserMarked;
            }
        }

        private async Task OnFinishedPostsScroll(TweetViewModel arg)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                await Task.Delay(100);

                var result = await GetPartOfTweetsFromUser(_userId, _lowerPostsDateTime, TimeDirection.Earlier);

                if (result.Count != 0)
                {
                    await AddTweetsFromUser(result, TimeDirection.Earlier);
                    await Task.Delay(100);
                }

                _isLoaded = false;
            }
        }

        private async Task OnFinishedLikesScroll(TweetViewModel arg)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                await Task.Delay(100);

                var result = await GetPartOfTweetsLikedUser(_userId, _lowerLikesDateTime, TimeDirection.Earlier);

                if (result.Count != 0)
                {
                    await AddTweetsLikedUser(result, TimeDirection.Earlier);
                    await Task.Delay(100);
                }

                _isLoaded = false;
            }
        }

        private async Task OnMutedProfilesTapCommand()
        {
            IsOutsideTappingEnabled = false;
            IsCurrentUserDetailVisible = false;
            await NavigationService.NavigateAsync(nameof(MutedListPage));
        }

        private async Task OnTapStoriesAsync()
        {
            if (UserStoryVM != null)
            {
                if ((!UserStoryVM.IsAuthorized && UserStoryVM.IsLoaded) || (UserStoryVM.IsAuthorized && UserStoryVM.IsLoaded))
                {
                    var parameters = new NavigationParameters();
                    parameters.Add(nameof(UserStory), UserStoryVM.ToUserStory());
                    var statusPermissionStorage = (await _permissionService.RequestGalleryPermissionAsync()).Result;
                    if (statusPermissionStorage == PermissionStatus.Granted)
                    {
                        await NavigationService.NavigateAsync(nameof(StoriesPage), parameters);
                    }
                }

                if (UserStoryVM.IsAuthorized && !UserStoryVM.IsLoaded)
                {
                    var statusPermissionCamera = (await _permissionService.RequestCameraPermissionAsync()).Result;
                    var statusPermissionMicrophone = (await _permissionService.RequestMicrophonePermissionAsync()).Result;

                    if (statusPermissionCamera == PermissionStatus.Granted && statusPermissionMicrophone == PermissionStatus.Granted)
                    {
                        await NavigationService.NavigateAsync(nameof(CameraPage));
                    }
                }
            }
        }

        #endregion
    }
}