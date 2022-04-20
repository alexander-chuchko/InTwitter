using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using InTwitter.Enums;
using InTwitter.Extensions;
using InTwitter.Helpers;
using InTwitter.Models.Notification;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Services.TweetService;
using InTwitter.Services.UserService;
using InTwitter.Models.Tweet;
using InTwitter.Models;
using InTwitter.Views;
using InTwitter.Models.User;

namespace InTwitter.ViewModels
{
    public class NotificationTabPageViewModel : ViewModelBase, IInitializeAsync
    {
        private readonly ITweetService _tweetService;
        private readonly IUserService _userService;
        private readonly IFeedService _feedService;
        private readonly IAuthorizationService _authorizationService;
        private bool _isLoaded = false;
        private DateTime _upperDateTime;
        private DateTime _lowerDateTime;

        public NotificationTabPageViewModel(
                                            INavigationService navigationService,
                                            ITweetService tweetService,
                                            IUserService userService,
                                            IFeedService feedService,
                                            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            Notifications = new ObservableCollection<NotificationViewModel>();

            _tweetService = tweetService;
            _userService = userService;
            _feedService = feedService;
            _authorizationService = authorizationService;

            _upperDateTime = DateTime.Now;
            _lowerDateTime = DateTime.Now;
            SeparatorVisibility = true;
        }

        #region -- Public properties --

        public ObservableCollection<NotificationViewModel> Notifications { get; set; }

        private double _scrollPosition;
        public double ScrollPosition
        {
            get => _scrollPosition;
            set => SetProperty(ref _scrollPosition, value);
        }

        private EFeedViewState _feedState;
        public EFeedViewState FeedState
        {
            get => _feedState;
            set => SetProperty(ref _feedState, value);
        }

        private bool separatorVisibility;
        public bool SeparatorVisibility
        {
            get => separatorVisibility;
            set => SetProperty(ref separatorVisibility, value);
        }

        public ICommand FinishedScrollCommand => SingleExecutionCommand.FromFunc(FinishedScrollHandler);

        public ICommand VideoLinkTapped => SingleExecutionCommand.FromFunc<TweetViewModel>(OnVideoLinkTapped);

        public ICommand TweetTapped => SingleExecutionCommand.FromFunc<TweetViewModel>(OnTweetTapped);

        public ICommand UserTapped => SingleExecutionCommand.FromFunc<UserViewModel>(OnUserTapped);

        #endregion

        #region ---IInitializeAsync Implementation---

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            AddNotifications(await GetPartOfNotifications(_authorizationService.CurrentUserId, DateTime.Now, TimeDirection.Earlier), TimeDirection.Earlier);
        }

        #endregion

        #region -- Private helpers --

        private async Task<List<Notification>> GetPartOfNotifications(Guid userId, DateTime timePoint, TimeDirection direction)
        {
            var result = await _feedService.GetNotificationsUser(userId, timePoint, direction);
            return result.Result;
        }

        private async Task AddNotifications(List<Notification> notifications, TimeDirection timeDirection)
        {
            if (notifications.Count != 0)
            {
                SeparatorVisibility = false;
                if (timeDirection == TimeDirection.Earlier)
                {
                    foreach (Notification notification in notifications)
                    {
                        if (_lowerDateTime > notification.CreationTime)
                        {
                            _lowerDateTime = notification.CreationTime;
                        }

                        Notifications.Add(notification.GetViewModel(_authorizationService.CurrentUserId, TextResources));
                        await Task.Delay(10);
                    }
                }

                if (timeDirection == TimeDirection.Later)
                {
                    foreach (Notification notification in notifications)
                    {
                        if (_upperDateTime < notification.CreationTime)
                        {
                            _upperDateTime = notification.CreationTime;
                        }

                        Notifications.Insert(0, notification.GetViewModel(_authorizationService.CurrentUserId, TextResources));
                    }
                }
            }
        }

        private async Task FinishedScrollHandler()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                FeedState = EFeedViewState.FeedLoading;

                await Task.Delay(100);

                var result = await GetPartOfNotifications(_authorizationService.CurrentUserId, _lowerDateTime, TimeDirection.Earlier);

                if (result.Count == 0)
                {
                    FeedState = EFeedViewState.NoResults;
                }
                else
                {
                    await AddNotifications(result, TimeDirection.Earlier);
                    await Task.Delay(100);
                }

                _isLoaded = false;
            }
        }

        private async Task OnVideoLinkTapped(TweetViewModel tweet)
        {
            if (tweet != null)
            {
                await NavigationService.NavigateAsync(nameof(TweetPage), new NavigationParameters
                {
                    { nameof(TweetViewModel), tweet },
                });
            }
        }

        private async Task OnTweetTapped(TweetViewModel tweet)
        {
            if (tweet != null)
            {
                await NavigationService.NavigateAsync(nameof(TweetPage), new NavigationParameters
                {
                   { nameof(TweetViewModel), tweet },
                });
            }
        }

        private async Task OnUserTapped(UserViewModel user)
        {
            if (user != null)
            {
                await NavigationService.NavigateAsync(nameof(ProfilePage), new NavigationParameters
                {
                    { nameof(User), user.Id },
                });
            }
        }

        #endregion
    }
}