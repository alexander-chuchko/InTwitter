using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using InTwitter.Models;
using InTwitter.Models.Tweet;
using InTwitter.Models.User;
using InTwitter.Enums;
using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.MediaService;
using InTwitter.Services.TweetService;
using InTwitter.Services.UserService;
using InTwitter.Views;
using InTwitter.Services.FeedService;

namespace InTwitter.ViewModels
{
    public class AddPostPageViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMediaService _mediaService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ITweetService _tweetService;

        private bool _isEnabledPostButton;

        private UserViewModel _profile;

        public AddPostPageViewModel(
            INavigationService navigationService,
            IUserService userService,
            IAuthorizationService authorizationService,
            IMediaService mediaService,
            IPageDialogService pageDialogService,
            ITweetService tweetService)
            : base(navigationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _mediaService = mediaService;
            _pageDialogService = pageDialogService;
            _tweetService = tweetService;

            MaxCount = Constants.MAX_LENGHT_TWEET;
            AddMediaState = EAddPostState.None;

            IsEnabledImageButton = true;
            IsEnabledVideoButton = true;
            _isEnabledPostButton = false;

            VideoImage = "ic_video.png";
            PhotoImage = "ic_media.png";

            PostButtonColor = (Color)App.Current.Resources["DarkGray"];

            MediaCollection = new List<MediaSourceViewModel>();
        }

        #region ---Public properties---

        private List<MediaSourceViewModel> _mediaCollection;

        public List<MediaSourceViewModel> MediaCollection
        {
            get => _mediaCollection;
            set => SetProperty(ref _mediaCollection, value);
        }

        private bool _isEnabledVideoButton;
        public bool IsEnabledVideoButton
        {
            get => _isEnabledVideoButton;
            set => SetProperty(ref _isEnabledVideoButton, value);
        }

        private bool _isEnabledImageButton;
        public bool IsEnabledImageButton
        {
            get => _isEnabledImageButton;
            set => SetProperty(ref _isEnabledImageButton, value);
        }

        private Color _postButtonColor;
        public Color PostButtonColor
        {
            get => _postButtonColor;
            set => SetProperty(ref _postButtonColor, value);
        }

        private string _videoImage;
        public string VideoImage
        {
            get => _videoImage;
            set => SetProperty(ref _videoImage, value);
        }

        private string _photoImage;
        public string PhotoImage
        {
            get => _photoImage;
            set => SetProperty(ref _photoImage, value);
        }

        private EAddPostState _addMediaState;
        public EAddPostState AddMediaState
        {
            get => _addMediaState;
            set => SetProperty(ref _addMediaState, value);
        }

        private string _profileIcon;
        public string ProfileIcon
        {
            get => _profileIcon;
            set => SetProperty(ref _profileIcon, value);
        }

        private int _symbolsCount;
        public int SymbolsCount
        {
            get => _symbolsCount;
            set => SetProperty(ref _symbolsCount, value);
        }

        private string _editorText;
        public string EditorText
        {
            get => _editorText;
            set => SetProperty(ref _editorText, value);
        }

        private int _maxCount;

        public int MaxCount
        {
            get => _maxCount;
            set => SetProperty(ref _maxCount, value);
        }

        private string _count;

        public string Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public ICommand DeleteImageTapCommand => SingleExecutionCommand.FromFunc(OnDeleteImageTapCommand);

        public ICommand PostButtonTapCommand => SingleExecutionCommand.FromFunc(OnPostButtonTapCommand);

        public ICommand AddVideoTapCommand => SingleExecutionCommand.FromFunc(OnAddVideoTapCommand);

        public ICommand GoBackTapCommand => SingleExecutionCommand.FromFunc(OnGoBackTap);

        public ICommand SymbolsCounterTapCommand => SingleExecutionCommand.FromFunc(OnSymbolsCounterTapCommand);

        public ICommand AddImageTapCommand => SingleExecutionCommand.FromFunc(OnAddImageTapCommand);

        public ICommand VideoDeleteTapCommand => SingleExecutionCommand.FromFunc(OnVideoDeleteTapCommand);

        #endregion

        #region ---Overrides---

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var user = await _userService.GetUserAsync(_authorizationService.CurrentUserId);

            if (user.IsSuccess)
            {
                _profile = user.Result.ToUserViewModel();

                if (!string.IsNullOrEmpty(_profile.IconSource))
                {
                    ProfileIcon = _profile.IconSource;
                }
                else
                {
                    ProfileIcon = "pic_profile_small.png";
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EditorText))
            {
                SymbolsCount = EditorText.Length;
                if (SymbolsCount > MaxCount)
                {
                    PostButtonColor = (Color)App.Current.Resources["DarkGray"];
                    _isEnabledPostButton = false;

                    if (SymbolsCount < MaxCount + 20)
                    {
                        Count = (MaxCount - SymbolsCount).ToString();
                    }
                    else
                    {
                        Count = ":D";
                    }
                }
                else
                {
                    if (SymbolsCount == 0)
                    {
                        PostButtonColor = (Color)App.Current.Resources["DarkGray"];
                        _isEnabledPostButton = false;
                    }
                    else
                    {
                        PostButtonColor = (Color)App.Current.Resources["PrimaryColor"];
                        _isEnabledPostButton = true;
                    }

                    Count = string.Empty;
                }
            }
        }

        #endregion

        #region ---Private helpers---

        private Task OnSymbolsCounterTapCommand()
        {
            if (EditorText.Length > MaxCount)
            {
                EditorText = EditorText.Remove(MaxCount);
            }

            return Task.CompletedTask;
        }

        private async Task OnGoBackTap()
        {
            if (!string.IsNullOrWhiteSpace(EditorText) || MediaCollection.Count != 0)
            {
                var allowGoBack = await _pageDialogService.DisplayAlertAsync(
                                               title: TextResources["Question"],
                                               message: TextResources["GoBackPostMessage"],
                                               acceptButton: TextResources["Confirm"],
                                               cancelButton: TextResources["Cancel"]);

                if (allowGoBack)
                {
                    await NavigationService.GoBackAsync();
                }
            }
            else
            {
                await NavigationService.GoBackAsync();
            }
        }

        private async Task OnAddImageTapCommand()
        {
            var result = await _mediaService.PickPhotoFromGalleryAsync();
            if (result.IsSuccess)
            {
                var media = new MediaSourceViewModel()
                {
                    MediaType = Enums.EMediaType.Image,
                    MediaSource = result.Result,
                };

                var newValue = new List<MediaSourceViewModel>(MediaCollection);
                newValue.Add(media);

                MediaCollection = newValue;

                VideoImage = "ic_video_disabled.png";
                IsEnabledVideoButton = false;

                if (MediaCollection.Count == 6)
                {
                    PhotoImage = "ic_media_disabled.png";
                    IsEnabledImageButton = false;
                }
                else
                {
                    PhotoImage = "ic_media.png";
                    IsEnabledImageButton = true;
                }

                AddMediaState = EAddPostState.Image;
            }
        }

        private async Task OnAddVideoTapCommand()
        {
            var result = await _mediaService.PickVideoFromGalleryAsync();
            if (result.IsSuccess)
            {
                var media = new MediaSourceViewModel()
                {
                    MediaType = Enums.EMediaType.Video,
                    MediaSource = result.Result,
                };

                VideoImage = "ic_video_disabled.png";
                PhotoImage = "ic_media_disabled.png";

                IsEnabledImageButton = false;
                IsEnabledVideoButton = false;

                AddMediaState = EAddPostState.Video;

                var newValue = new List<MediaSourceViewModel>(MediaCollection);
                newValue.Add(media);

                MediaCollection = newValue;
            }
        }

        private Task OnVideoDeleteTapCommand()
        {
            var newValue = new List<MediaSourceViewModel>();
            MediaCollection = newValue;

            AddMediaState = EAddPostState.None;

            VideoImage = "ic_video.png";
            PhotoImage = "ic_media.png";

            IsEnabledImageButton = true;
            IsEnabledVideoButton = true;

            return Task.CompletedTask;
        }

        private Task OnDeleteImageTapCommand(object sender)
        {
            MediaSourceViewModel mediaVM = sender as MediaSourceViewModel;

            var newValue = new List<MediaSourceViewModel>(MediaCollection);
            newValue.Remove(mediaVM);

            MediaCollection = newValue;

            if (MediaCollection.Count == 0)
            {
                VideoImage = "ic_video.png";
                IsEnabledVideoButton = true;

                AddMediaState = EAddPostState.None;
            }

            IsEnabledImageButton = true;
            PhotoImage = "ic_media.png";

            return Task.CompletedTask;
        }

        private async Task OnPostButtonTapCommand()
        {
            if (_isEnabledPostButton)
            {
                bool allowGoBack = await _pageDialogService.DisplayAlertAsync(
                                                title: "Question",
                                                message: "Are you sure that you want to post?",
                                                acceptButton: "Confirm",
                                                cancelButton: "Cancel");

                if (allowGoBack)
                {
                    TweetViewModel tweetVM = new TweetViewModel()
                    {
                        Id = Guid.NewGuid(),
                        Text = EditorText,
                        CreationTime = DateTime.Now,
                        User = _profile,
                    };

                    var coll = new List<TweetMediaSource>();

                    foreach (var item in MediaCollection)
                    {
                        item.TweetId = tweetVM.Id;
                        coll.Add(item.ToTweetMediaSource());
                    }

                    await _tweetService.CreateTweetAsync(tweetVM.ToTweet(), coll);

                    await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}");
                }
            }
        }

        #endregion
    }
}