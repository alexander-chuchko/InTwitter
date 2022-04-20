using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.Mocks;
using InTwitter.Models.Icon;
using InTwitter.Models.Stories;
using InTwitter.Models.Tweet;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Services.MediaService;
using InTwitter.Services.PermissionService;
using InTwitter.Services.StoriesService;
using InTwitter.Services.TweetService;
using InTwitter.Services.UserService;
using InTwitter.Views;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InTwitter.ViewModels
{
    public class FeedViewModel : BaseFeedPageViewModel
    {
        private readonly IMediaService _mediaService;
        private readonly IStoriesService _storiesService;
        private readonly IPermissionService _permissionService;

        public FeedViewModel(INavigationService navigationService, IAuthorizationService authorizationService, IFeedService feedService, ITweetService tweetService, IUserService userService, IMediaService mediaService, IStoriesService storiesService, IPermissionService permissionService)
            : base(navigationService, authorizationService, feedService, tweetService, userService)
        {
            this._mediaService = mediaService;
            this._storiesService = storiesService;
            this._permissionService = permissionService;
        }

        #region ---Public properties---

        private ObservableCollection<UserStoryViewModel> _UserStoriesVM;
        public ObservableCollection<UserStoryViewModel> UserStoriesVM
        {
            get => _UserStoriesVM;
            set => SetProperty(ref _UserStoriesVM, value);
        }

        private UserStory _SelectedIconStory;

        public UserStory SelectedUserStory
        {
            get => _SelectedIconStory;
            set => SetProperty(ref _SelectedIconStory, value);
        }

        private bool _Alive = true;
        public bool Alive
        {
            get { return _Alive; }
            set { SetProperty(ref _Alive, value); }
        }

        public ICommand TapUserStroryCommand => SingleExecutionCommand.FromFunc(OnTapUserStory);

        #endregion

        #region ---  Overrides  ---
        protected override async Task<List<Tweet>> GetPartOfTweets(Guid userId, DateTime timePoint, TimeDirection direction)
        {
            var result = await FeedService.GetFeedToUser(userId, timePoint, direction);
            return result.Result;
        }

        protected async override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SelectedUserStory))
            {
                if (SelectedUserStory.UserId == _currentUserId)
                {
                    await OnNavigationToViewStoriesPage();
                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Alive = false;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (UserStoriesVM == null)
            {
                var getAllStoriesResult = await _storiesService.GetAllStoriesModelAsync<UserStories>();

                if (!getAllStoriesResult.IsAny())
                {
                    await CreateAndAddUserStoryToDataBase();
                    getAllStoriesResult = await _storiesService.GetAllStoriesModelAsync<UserStories>();
                }

                await UpdateStoriesList();

                Device.StartTimer(TimeSpan.FromSeconds(20), () =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await UpdateStoriesList();
                    });

                    return Alive;
                });
            }
        }

        #endregion

        #region --- Private helpers ---

        private async Task CreateAndAddUserStoryToDataBase()
        {
            MocksGenerator.GenerateUserStories(_currentUserId);

            foreach (var userStoryState in UserStoriesState.Instance.UserStories)
            {
                await _storiesService.SaveStoriesModelAsync(userStoryState);
            }

            foreach (var mediaStoriesSourceState in MediaStoriesSourceState.Instance.MediaStoriesSources)
            {
                await _storiesService.SaveStoriesModelAsync(mediaStoriesSourceState);
            }

            foreach (var postData in PostDataState.Instance.PostDatas)
            {
                await _storiesService.SaveStoriesModelAsync(postData);
            }
        }

        private async Task<bool> RemoveMediaFromDataBase()
        {
            bool operationResult = true;
            var getAllMediaStoriesResult = await _storiesService.GetAllStoriesModelAsync<MediaStoriesSource>();
            var getStoriesResult = await _storiesService.GetByIdStoriesModelAsync<UserStories>(_currentUserId.ToString());

            if (getAllMediaStoriesResult.IsAny())
            {
                foreach (var mediaStoriesSource in getAllMediaStoriesResult)
                {
                    if (operationResult)
                    {
                        TimeSpan difference = DateTimeOffset.Now - mediaStoriesSource.PublicationTime;

                        if (difference.TotalMinutes >= Constants.LIFETIME_OF_POSTIN_STORY)
                        {
                            foreach (var item in getStoriesResult.PostData)
                            {
                                if (item.UserId == mediaStoriesSource.Owner.Id && item.LastMediaElement > 0)
                                {
                                    var updatedObject = new PostData()
                                    {
                                        Id = item.Id,
                                        LastMediaElement = item.LastMediaElement - 1,
                                        UserId = item.UserId,
                                        Owner = getStoriesResult,
                                    };

                                    await _storiesService.SaveStoriesModelAsync<PostData>(updatedObject, update: true);
                                }
                            }

                            await _storiesService.DeleteStoriesModelAsync(mediaStoriesSource);
                        }
                    }
                }
            }
            else
            {
                Alive = false;
            }

            return operationResult;
        }

        private async Task<bool> RemoveStoriesFromDataBase()
        {
            bool operationResult = true;
            var listUserStories = await _storiesService.GetAllStoriesModelAsync<UserStories>();

            foreach (var userStories in listUserStories)
            {
                if (operationResult)
                {
                    if (!userStories.MediaStoriesSources.IsAny() && _currentUserId != Guid.Parse(userStories.Id))
                    {
                        bool result = false;
                        if (UserStoriesVM.IsAny())
                        {
                            result = RemoveItemFromUserStoriesViewModel(userStories);
                        }

                        if (result)
                        {
                            operationResult = await _storiesService.DeleteObjectAndItsDependencies<UserStories>(userStories);
                        }
                    }
                }
            }

            return operationResult;
        }

        private bool RemoveItemFromUserStoriesViewModel(UserStories userStories)
        {
            bool operationResult = false;
            var story = UserStoriesVM.Where(x => x.Id == Guid.Parse(userStories.Id)).FirstOrDefault();

            if (story != null && _currentUserId != Guid.Parse(userStories.Id))
            {
                var userStoriesViewModel = UserStoriesVM;
                userStoriesViewModel.Remove(story);
                UserStoriesVM = userStoriesViewModel;
                operationResult = true;
            }

            return operationResult;
        }

        private async Task CreateStoriesViewModel()
        {
            var getAllStoriesResult = await _storiesService.GetAllStoriesModelAsync<UserStories>();
            var getAllIdsFollowersResult = await _storiesService.GetIdsFollowers<UserStories>();
            ObservableCollection<UserStoryViewModel> listUserStoriesViewModel = null;

            if (getAllStoriesResult.IsAny())
            {
                listUserStoriesViewModel = new ObservableCollection<UserStoryViewModel>(getAllStoriesResult.Where(s => s.Id != _currentUserId.ToString()).Select(s => s.ToUserStoryViewModelFromUserStories(getAllIdsFollowersResult)));
                var storiesAuthorizationUser = getAllStoriesResult.Where(s => s.Id == _currentUserId.ToString()).Select(s => s.ToUserStoryViewModelFromUserStories(null)).FirstOrDefault();

                if (storiesAuthorizationUser != null)
                {
                    listUserStoriesViewModel.Insert(0, storiesAuthorizationUser);
                }

                UserStoriesVM = listUserStoriesViewModel;
            }
        }

        private async Task UpdateStoriesList()
        {
            if (await RemoveMediaFromDataBase() && await RemoveStoriesFromDataBase())
            {
                if (!UserStoriesVM.IsAny())
                {
                    await CreateStoriesViewModel();
                }
            }
        }

        private async Task OnTapUserStory(object parametr)
        {
            if (parametr is UserStoryViewModel userStoryViewModel)
            {
                Alive = false;

                if (!userStoryViewModel.IsAuthorized || (userStoryViewModel.IsAuthorized && userStoryViewModel.IsLoaded))
                {
                    var parameters = new NavigationParameters();
                    parameters.Add(nameof(UserStory), userStoryViewModel.ToUserStory());
                    var statusPermissionStorage = (await _permissionService.RequestGalleryPermissionAsync()).Result;

                    if (statusPermissionStorage == PermissionStatus.Granted)
                    {
                        await NavigationService.NavigateAsync(nameof(StoriesPage), parameters);
                    }
                }

                if (userStoryViewModel.IsAuthorized && !userStoryViewModel.IsLoaded)
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

        private async Task OnNavigationToViewStoriesPage()
        {
            await NavigationService.NavigateAsync(nameof(StoriesPage));
        }

        #endregion
    }
}