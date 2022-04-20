using InTwitter.Helpers;
using InTwitter.Models.Stories;
using InTwitter.PlatformDependencyInterface;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.MediaService;
using InTwitter.Services.PermissionService;
using InTwitter.Services.StoriesService;
using InTwitter.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace InTwitter.ViewModels
{
    public class CameraPageViewModel : ViewModelBase
    {
        private readonly IMediaService _mediaService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPermissionService _permissionService;
        private readonly IStoriesService _storiesService;
        private readonly IAuthorizationService _authorizationService;
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;

        public CameraPageViewModel(
            INavigationService navigationService,
            IMediaService mediaService,
            IPageDialogService pageDialogService,
            IPermissionService permissionService,
            IStoriesService storiesService,
            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _mediaService = mediaService;
            _permissionService = permissionService;
            _storiesService = storiesService;
            _authorizationService = authorizationService;
            CameraMode = CameraOptions.Back;
            CaptureMode = CameraCaptureMode.Video;
            VisableImageCamera = false;
            PageStatus = false;
            IsRecording = true;
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;
        }

        #region --- Public Properties ---

        private bool _IsRecording;
        public bool IsRecording
        {
            get => _IsRecording;
            set => SetProperty(ref _IsRecording, value);
        }

        private bool _VisableImageCamera;
        public bool VisableImageCamera
        {
            get => _VisableImageCamera;
            set => SetProperty(ref _VisableImageCamera, value);
        }

        private UserStories _UserStories;
        public UserStories UserStories
        {
            get => _UserStories;
            set => SetProperty(ref _UserStories, value);
        }

        private string _PathImageSource;
        public string PathImageSource
        {
            get => _PathImageSource;
            set => SetProperty(ref _PathImageSource, value);
        }

        private string _VideoSource;
        public string VideoSource
        {
            get => _VideoSource;
            set => SetProperty(ref _VideoSource, value);
        }

        private byte[] _ImageData;
        public byte[] ImageData
        {
            get => _ImageData;
            set => SetProperty(ref _ImageData, value);
        }

        private string _PathMediaSource;
        public string PathMediaSource
        {
            get { return _PathMediaSource; }
            set { SetProperty(ref _PathMediaSource, value); }
        }

        private CameraCaptureMode _CaptureMode;
        public CameraCaptureMode CaptureMode
        {
            get => _CaptureMode;
            set => SetProperty(ref _CaptureMode, value);
        }

        private MediaElementState _MediaElementMode;
        public MediaElementState MediaElementMode
        {
            get => _MediaElementMode;
            set => SetProperty(ref _MediaElementMode, value);
        }

        private bool _OpenShutter;
        public bool OpenShutter
        {
            get => _OpenShutter;
            set => SetProperty(ref _OpenShutter, value);
        }

        private CameraOptions _CameraMode;
        public CameraOptions CameraMode
        {
            get => _CameraMode;
            set => SetProperty(ref _CameraMode, value);
        }

        private bool _PageStatus;
        public bool PageStatus
        {
            get => _PageStatus;
            set => SetProperty(ref _PageStatus, value);
        }

        private DateTimeOffset _Timer;
        public DateTimeOffset Timer
        {
            get => _Timer;
            set => SetProperty(ref _Timer, value);
        }

        public ICommand TapFlipCameraOrPostCommand => SingleExecutionCommand.FromFunc(OnTapFlipCameraOrPostAsync);

        public ICommand TapCloseCommand => SingleExecutionCommand.FromFunc(OnTapCloseAsync);

        public ICommand AddImageTapCommand => SingleExecutionCommand.FromFunc(OnAddMediaFromGalleryAsync);

        private ICommand _PressedButtonCommand;
        public ICommand PressedButtonCommand => SingleExecutionCommand.FromFunc(OnPressedButtonAsync);

        #endregion

        #region --- Overrides ---

        protected async override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(ImageData))
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var result = await _mediaService.SaveFromStreamToImage(ImageData, 0, 0, false, CameraMode);
                    if (result.IsSuccess)
                    {
                        PathImageSource = result.Result;
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    var saver = DependencyService.Get<IImageSaver>();
                    var result = await saver.SaveFromStreamToImage(ImageData, 0, 0, true, CameraMode);
                    if (result != null)
                    {
                        PathImageSource = result;
                    }
                }
            }
            else if (args.PropertyName == nameof(VideoSource))
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var resultPath = _mediaService.CreatePath();

                    if (resultPath != null)
                    {
                        var resultVideo = await _mediaService.TrimOrRotateVideoAsync(VideoSource, resultPath, 0, 0, CameraMode);
                        if (resultVideo.IsSuccess)
                        {
                            PathImageSource = resultPath;
                        }
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    PathImageSource = VideoSource;
                }
            }
        }

        #endregion

        #region --- Private helpers ---

        private async Task OnTapCloseAsync()
        {
            if (!PageStatus)
            {
                await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}");
            }
            else
            {
                var confirmConfig = await _pageDialogService.DisplayAlertAsync(
                title: "Delete story",
                message: "You haven't published a story, if you leav the page, your story will be deleted",
                acceptButton: TextResources["Remove"],
                cancelButton: TextResources["Cancel"]);

                if (confirmConfig)
                {
                    MediaElementMode = MediaElementState.Stopped;
                    PageStatus = false;
                }
            }
        }

        private async Task OnTapFlipCameraOrPostAsync(object parametr)
        {
            if (!PageStatus)
            {
                await SwitchCamera();
            }
            else
            {
                UserStories = await _storiesService.GetByIdStoriesModelAsync<UserStories>(_authorizationService.CurrentUserId.ToString());

                if (UserStories == null)
                {
                    UserStories userStories = new UserStories()
                    {
                        Id = _authorizationService.CurrentUserId.ToString(),
                        LastUpdatedPost = DateTimeOffset.Now,
                    };

                    bool resultSave = await _storiesService.SaveStoriesModelAsync(userStories);

                    if (resultSave)
                    {
                        UserStories = await _storiesService.GetByIdStoriesModelAsync<UserStories>(_authorizationService.CurrentUserId.ToString());
                    }
                }

                if (UserStories != null)
                {
                    bool resultIsVideo = CaptureMode
                        != CameraCaptureMode.Photo
                        ? true
                        : false;

                    MediaStoriesSource mediaStoriesSource = new MediaStoriesSource()
                    {
                        Owner = UserStories,
                        MediaSource = PathImageSource,
                        IsVideo = resultIsVideo,
                        PublicationTime = DateTimeOffset.Now,
                    };

                    bool resultSaveOfMediaSource = await _storiesService.SaveStoriesModelAsync(mediaStoriesSource, update: false);

                    PostData postData = new PostData()
                    {
                        Owner = UserStories,
                        LastMediaElement = default(int),
                        UserId = UserStories.Id,
                    };

                    bool resultSaveOfPostData = await _storiesService.SaveStoriesModelAsync(postData, update: false);

                    if (resultSaveOfMediaSource && resultSaveOfPostData)
                    {
                        UserStories = new UserStories()
                        {
                            Id = _authorizationService.CurrentUserId.ToString(),
                            LastUpdatedPost = DateTimeOffset.Now,
                        };
                    }

                    bool resultOperationUpdate = await _storiesService.SaveStoriesModelAsync(UserStories, update: true);

                    if (resultOperationUpdate)
                    {
                        await NavigationService.NavigateAsync($"/{nameof(MainMasterDetailPage)}/{nameof(NavigationPage)}/{nameof(MainTabbedPage)}");
                    }
                }
            }
        }

        private async Task SwitchCamera()
        {
            if (CameraMode != CameraOptions.Front)
            {
                CameraMode = CameraOptions.Front;
                await Task.Delay(500);
            }
            else
            {
                CameraMode = CameraOptions.Default;
                await Task.Delay(500);
            }
        }

        private async Task OnAddMediaFromGalleryAsync()
        {
            bool resultOperation = false;
            var resultSelection = await _pageDialogService.DisplayActionSheetAsync(
            title: null,
            cancelButton: TextResources["Cancel"],
            destroyButton: null,
            Constants.TRANSITION_TO_PICTURE_RESOURCES,
            Constants.TRANSITION_TO_VIDEO_RESOURCES);

            if (resultSelection == Constants.TRANSITION_TO_PICTURE_RESOURCES)
            {
                resultOperation = await GetMediaSource(Constants.TRANSITION_TO_PICTURE_RESOURCES);

                if (resultOperation)
                {
                    await AddToDataBaseMediaSource(Constants.TRANSITION_TO_PICTURE_RESOURCES);
                }
            }
            else if (resultSelection == Constants.TRANSITION_TO_VIDEO_RESOURCES)
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
                    var selectedMediaSource = await _mediaService.PickVideoFromGalleryAsync();

                    if (selectedMediaSource.IsSuccess)
                    {
                        var durationVideo = await _mediaService.DetermineVideoDurationAsync(selectedMediaSource.Result);

                        if (durationVideo.IsSuccess)
                        {
                            if (durationVideo.Result > Constants.DURATION_PREVIEW_VIDEO)
                            {
                                string outputPath = _mediaService.CreatePath();
                                var resultOperation = await _mediaService.TrimOrRotateVideoAsync(selectedMediaSource.Result, outputPath, 0, 30000);

                                if (resultOperation.IsSuccess)
                                {
                                    PathMediaSource = outputPath;
                                }
                            }
                            else
                            {
                                PathMediaSource = selectedMediaSource.Result;
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

        private async Task OnPressedButtonAsync(object parametr)
        {
            var resultPressedButton = parametr.ToString();

            if (!string.IsNullOrWhiteSpace(resultPressedButton))
            {
                switch (resultPressedButton)
                {
                    case Constants.PRESSED_BUTTON:

                        if (CaptureMode != CameraCaptureMode.Photo)
                        {
                            CaptureMode = CameraCaptureMode.Photo;
                            await Task.Delay(500);
                        }

                        break;

                    case Constants.LONG_PRESSED_BUTTON:

                        if (CaptureMode != CameraCaptureMode.Video)
                        {
                            CaptureMode = CameraCaptureMode.Video;
                            await Task.Delay(500);
                        }

                        break;
                }

                OpenShutter = true;

                if (CaptureMode == CameraCaptureMode.Video && IsRecording)
                {
                    if (token.IsCancellationRequested)
                    {
                        cancelTokenSource.Dispose();
                        cancelTokenSource = null;
                    }

                    if (cancelTokenSource == null)
                    {
                        cancelTokenSource = new CancellationTokenSource();
                        token = cancelTokenSource.Token;
                    }

                    await StartTimer(token);
                }

                if (CaptureMode == CameraCaptureMode.Video && !IsRecording)
                {
                    cancelTokenSource.Cancel();
                    Timer = DateTime.UnixEpoch;
                }
            }
        }

        public async Task StartTimer(CancellationToken token)
        {
            await Task.Run(() =>
            {
                DateTimeOffset dateTime = DateTimeOffset.UnixEpoch;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    dateTime = dateTime.AddSeconds(1);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        Timer = dateTime;
                    });

                    return IsRecording;
                });
            });
        }

        #endregion
    }
}
