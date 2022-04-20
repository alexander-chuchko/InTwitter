using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.Models.Icon;
using InTwitter.Models.MediaSource;
using InTwitter.Models.Stories;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.MediaService;
using InTwitter.Services.StoriesService;
using InTwitter.Services.UserService;
using InTwitter.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.ViewModels
{
    public class StoriesPageViewModel : ViewModelBase
    {
        private readonly IStoriesService _storiesService;
        private readonly IMediaService _mediaService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IUserService _userService;

        public StoriesPageViewModel(
            INavigationService navigationService,
            IStoriesService storiesService,
            IMediaService mediaService,
            IAuthorizationService authorizationService,
            IPageDialogService pageDialogService,
            IUserService userService)
            : base(navigationService)
        {
            this._storiesService = storiesService;
            this._mediaService = mediaService;
            this._authorizationService = authorizationService;
            this._pageDialogService = pageDialogService;
            this._userService = userService;
            Token = CancelToken.Token;
        }

        #region --- Public properties ---

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private string _UserPictureSource;
        public string UserPictureSource
        {
            get { return _UserPictureSource; }
            set { SetProperty(ref _UserPictureSource, value); }
        }

        private List<MediaStorySource> _MediaStorySources;
        public List<MediaStorySource> MediaStorySources
        {
            get { return _MediaStorySources; }
            set { SetProperty(ref _MediaStorySources, value); }
        }

        private bool _PageStatus;
        public bool PageStatus
        {
            get => _PageStatus;
            set => SetProperty(ref _PageStatus, value);
        }

        private MediaStorySource _MediaStorySource;
        public MediaStorySource MediaStorySource
        {
            get => _MediaStorySource;
            set => SetProperty(ref _MediaStorySource, value);
        }

        private UserStoryViewModel _UserStoryVM;
        public UserStoryViewModel UserStoryVM
        {
            get => _UserStoryVM;
            set => SetProperty(ref _UserStoryVM, value);
        }

        private CancellationToken _Token;
        public CancellationToken Token
        {
            get => _Token;
            set => SetProperty(ref _Token, value);
        }

        private CancellationTokenSource _CancelToken = new CancellationTokenSource();
        public CancellationTokenSource CancelToken
        {
            get => _CancelToken;
            set => SetProperty(ref _CancelToken, value);
        }

        private string _PathMediaSource;
        public string PathMediaSource
        {
            get { return _PathMediaSource; }
            set { SetProperty(ref _PathMediaSource, value); }
        }

        private (double, double) _StateProgressBar = (0.0, 0.0);
        public (double, double) StateProgressBar
        {
            get { return _StateProgressBar; }
            set { SetProperty(ref _StateProgressBar, value); }
        }

        public ICommand TapCommand => SingleExecutionCommand.FromFunc(OnTap);

        #endregion

        #region --- Overrides ---

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(nameof(UserStory)))
            {
                var userStory = parameters[nameof(UserStory)] as UserStory;
                await LoadUserAsync(userStory);
            }
        }

        #endregion

        #region --- Private Helpers ---

        private async Task LoadUserAsync(UserStory userStory)
        {
            UserStoryVM = userStory.ToUserStoryViewModel();
            var getStoriesResult = await _storiesService.GetByIdStoriesModelAsync<UserStories>(UserStoryVM.Id.ToString());
            var userStories = new List<MediaStorySource>();
            var getStoriesAuthorizedUser = await _storiesService.GetByIdStoriesModelAsync<UserStories>(_authorizationService.CurrentUserId.ToString());

            if (getStoriesResult != null)
            {
                Name = await _userService.GetNameAsync(UserStoryVM.Id);
                UserPictureSource = UserStoryVM.UserIcon;

                foreach (var storiesModel in getStoriesResult.MediaStoriesSources.OrderBy(pb => pb.PublicationTime))
                {
                    MediaStorySource resultMediaStorySource = null;

                    if (storiesModel.IsVideo)
                    {
                        var durationVideo = await _mediaService.DetermineVideoDurationAsync(storiesModel.MediaSource);

                        if (durationVideo.IsSuccess)
                        {
                            resultMediaStorySource = storiesModel.ToMediaStorySource(durationVideo.Result);
                        }
                    }
                    else
                    {
                        resultMediaStorySource = storiesModel.ToMediaStorySource();
                    }

                    if (resultMediaStorySource != null)
                    {
                        userStories.Add(resultMediaStorySource);
                    }
                }

                if (UserStoryVM.Id != _authorizationService.CurrentUserId)
                {
                    PageStatus = false;
                }
                else
                {
                    PageStatus = true;
                }

                if (getStoriesAuthorizedUser.PostData.ToList().Count != 0)
                {
                    foreach (var postData in getStoriesAuthorizedUser.PostData)
                    {
                        if (postData.UserId == UserStoryVM.Id.ToString())
                        {
                            int count = postData.LastMediaElement
                                == userStories.Count
                                ? 0
                                : postData.LastMediaElement;
                            MediaStorySource = userStories[count];
                        }
                    }
                }

                MediaStorySources = userStories;
            }
        }

        private async Task OnTap(object parametr)
        {
            string commandParametr = parametr as string;

            if (commandParametr != null)
            {
                int index = MediaStorySources.IndexOf(MediaStorySource);

                switch (commandParametr)
                {
                    case Constants.CLOSING_PAGE:

                        await CancelThread();

                        if (!PageStatus)
                        {
                            await SaveInformationItem(index);
                        }

                        await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}"); //to do

                        break;

                    case Constants.GO_TO_CAMERA_PAGE:

                        await CancelThread();
                        await NavigationService.NavigateAsync(nameof(CameraPage));

                        break;

                    case Constants.GO_TO_STORAGE_MEDIA:

                        await CancelThread();
                        InitializationCancelToken();
                        await GetMediaFromGallery();

                        break;

                    case Constants.REMOVE_ITEM_FROM_STORIES:

                        await CancelThread();

                        await RemoveItemFromDataBase();

                        InitializationCancelToken();

                        var userStoriesList = await _storiesService.GetByIdStoriesModelAsync<UserStories>(UserStoryVM.Id.ToString());

                        if (!userStoriesList.MediaStoriesSources.IsAny())
                        {
                            await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}");
                        }

                        await LoadStoriesUser(userStoriesList);

                        break;
                }
            }
        }

        private async Task SaveInformationItem(int index)
        {
            var getUserStoriesResult = await _storiesService.GetByIdStoriesModelAsync<UserStories>(_authorizationService.CurrentUserId.ToString());

            foreach (var postData in getUserStoriesResult.PostData)
            {
                if (postData.UserId == UserStoryVM.Id.ToString())
                {
                    if (postData.LastMediaElement != MediaStorySources.Count)
                    {
                        var updatedObject = new PostData()
                        {
                            Id = postData.Id,
                            LastMediaElement = index + 1,
                            UserId = UserStoryVM.Id.ToString(),
                            Owner = getUserStoriesResult,
                        };

                        await _storiesService.SaveStoriesModelAsync<PostData>(updatedObject, update: true);
                    }
                }
            }
        }

        private async Task GetMediaFromGallery()
        {
            bool resultOperation = false;
            var resultAction = await _pageDialogService.DisplayActionSheetAsync(
                        title: null,
                        cancelButton: TextResources["Cancel"],
                        destroyButton: null,
                        Constants.TRANSITION_TO_PICTURE_RESOURCES,
                        Constants.TRANSITION_TO_VIDEO_RESOURCES);

            if (resultAction == Constants.TRANSITION_TO_PICTURE_RESOURCES)
            {
                resultOperation = await GetMediaSource(Constants.TRANSITION_TO_PICTURE_RESOURCES);

                if (resultOperation)
                {
                    await AddToDataBaseMediaSource(Constants.TRANSITION_TO_PICTURE_RESOURCES);
                }
            }
            else if (resultAction == Constants.TRANSITION_TO_VIDEO_RESOURCES)
            {
                resultOperation = await GetMediaSource(Constants.TRANSITION_TO_VIDEO_RESOURCES);

                if (resultOperation)
                {
                    await AddToDataBaseMediaSource(Constants.TRANSITION_TO_VIDEO_RESOURCES);
                }
            }

            if (resultOperation)
            {
                await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}");
            }
            else
            {
                var getUserStoriesResult = await _storiesService.GetByIdStoriesModelAsync<UserStories>(UserStoryVM.Id.ToString());
                var mediaStoriesList = new List<MediaStorySource>();

                foreach (var mediaStory in getUserStoriesResult.MediaStoriesSources)
                {
                    MediaStorySource mediaStorySource = null;

                    if (mediaStory.IsVideo)
                    {
                        var durationVideo = await _mediaService.DetermineVideoDurationAsync(mediaStory.MediaSource);

                        if (durationVideo.IsSuccess)
                        {
                            mediaStorySource = mediaStory.ToMediaStorySource(durationVideo.Result);
                        }
                    }
                    else
                    {
                        mediaStorySource = mediaStory.ToMediaStorySource();
                    }

                    if (mediaStorySource != null)
                    {
                        mediaStoriesList.Add(mediaStorySource);
                    }
                }

                MediaStorySources = mediaStoriesList;
            }
        }

        private async Task RemoveItemFromDataBase()
        {
            var confirmConfig = await _pageDialogService.DisplayAlertAsync(
                title: "Delete story",
                message: "The story will be deleted and cannot and cannot be restored",
                acceptButton: TextResources["Remove"],
                cancelButton: TextResources["Cancel"]);

            if (confirmConfig)
            {
                var mediaStoriesSourceList = await _storiesService.GetByIdStoriesModelAsync<MediaStoriesSource>(MediaStorySource.Id.ToString());

                bool operationResult = false;
                if (mediaStoriesSourceList.Id == MediaStorySource.Id.ToString())
                {
                    operationResult = await _storiesService.DeleteStoriesModelAsync(mediaStoriesSourceList);

                    if (operationResult)
                    {
                        StateProgressBar = (0.0, 0.0);
                    }
                }
            }
        }

        private async Task LoadStoriesUser(UserStories userStoriesList)
        {
            var mediaStorySourceList = new List<MediaStorySource>();

            foreach (var mediaStory in userStoriesList.MediaStoriesSources)
            {
                MediaStorySource resultMediaStorySource = null;

                if (mediaStory.IsVideo)
                {
                    var durationVideo = await _mediaService.DetermineVideoDurationAsync(mediaStory.MediaSource);

                    if (durationVideo.IsSuccess)
                    {
                        resultMediaStorySource = mediaStory.ToMediaStorySource(durationVideo.Result);
                    }
                }
                else
                {
                    resultMediaStorySource = mediaStory.ToMediaStorySource();
                }

                if (resultMediaStorySource != null)
                {
                    mediaStorySourceList.Add(resultMediaStorySource);
                }
            }

            MediaStorySources = mediaStorySourceList;
        }

        private async Task CancelThread()
        {
            CancelToken.Cancel();
            await Task.Delay(100);
            CancelToken.Dispose();
            CancelToken = null;
        }

        private void InitializationCancelToken()
        {
            if (CancelToken == null)
            {
                CancelToken = new CancellationTokenSource();
            }
        }

        private async Task<bool> GetMediaSource(string parametr)
        {
            bool resultReceiving = false;

            if (!string.IsNullOrEmpty(parametr))
            {
                if (parametr == Constants.TRANSITION_TO_PICTURE_RESOURCES)
                {
                    var resultPickPhoto = await _mediaService.PickPhotoFromGalleryAsync();

                    if (resultPickPhoto.IsSuccess)
                    {
                        PathMediaSource = resultPickPhoto.Result;
                        resultReceiving = true;
                    }
                }
                else if (parametr == Constants.TRANSITION_TO_VIDEO_RESOURCES)
                {
                    var result = await _mediaService.PickVideoFromGalleryAsync();

                    if (result.IsSuccess)
                    {
                        var durationVideo = await _mediaService.DetermineVideoDurationAsync(result.Result);

                        if (durationVideo.IsSuccess)
                        {
                            if (durationVideo.Result > Constants.DURATION_PREVIEW_VIDEO)
                            {
                                string outputPath = _mediaService.CreatePath();
                                var resultOperation = await _mediaService.TrimOrRotateVideoAsync(result.Result, outputPath, 0, 30000);

                                if (resultOperation.IsSuccess)
                                {
                                    PathMediaSource = outputPath;
                                }
                            }
                            else
                            {
                                PathMediaSource = result.Result;
                            }

                            resultReceiving = true;
                        }
                    }
                }
            }

            return resultReceiving;
        }

        private async Task AddToDataBaseMediaSource(string parametr)
        {
            var resultUserStories = await _storiesService.GetByIdStoriesModelAsync<UserStories>(_authorizationService.CurrentUserId.ToString());

            if (resultUserStories != null)
            {
                bool resultIsVideo = parametr
                    == Constants.TRANSITION_TO_VIDEO_RESOURCES
                    ? true
                    : false;

                MediaStoriesSource mediaStoriesSource = new MediaStoriesSource()
                {
                    Owner = resultUserStories,
                    MediaSource = PathMediaSource,
                    IsVideo = resultIsVideo,
                    PublicationTime = DateTimeOffset.Now,
                };

                await _storiesService.SaveStoriesModelAsync(mediaStoriesSource, update: false);

                PostData postData = new PostData()
                {
                    Owner = resultUserStories,
                    LastMediaElement = default(int),
                    UserId = resultUserStories.Id,
                };

                await _storiesService.SaveStoriesModelAsync(postData, update: false);
            }
        }

        #endregion
    }
}
