using InTwitter.Models.Tweet;
using InTwitter.Helpers;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.FeedService;
using InTwitter.Services.TweetService;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InTwitter.Extensions;
using System.ComponentModel;
using InTwitter.Services.UserService;
using InTwitter.Services.StoriesService;

namespace InTwitter.ViewModels
{
    public class BookmarksTabPageViewModel : BaseFeedPageViewModel
    {
        private readonly IPageDialogService _pageDialog;
        private readonly ITweetService _tweetService;

        public BookmarksTabPageViewModel(
                                         INavigationService navigationService,
                                         IAuthorizationService authorizationService,
                                         IFeedService feedService,
                                         IPageDialogService pageDialog,
                                         ITweetService tweetService,
                                         IUserService userService)
            : base(navigationService, authorizationService, feedService, tweetService, userService)
        {
            _pageDialog = pageDialog;
            _tweetService = tweetService;
            IsDotsImageVisible = true;
            IsDeleteMenuVisible = false;
        }

        #region -- Public properties --

        private bool isDotsImageVisible;
        public bool IsDotsImageVisible
        {
            get => isDotsImageVisible;
            set => SetProperty(ref isDotsImageVisible, value);
        }

        private bool isDeleteMenuVisible;
        public bool IsDeleteMenuVisible
        {
            get => isDeleteMenuVisible;
            set => SetProperty(ref isDeleteMenuVisible, value);
        }

        private bool isGridVisible;
        public bool IsGridVisible
        {
            get => isGridVisible;
            set => SetProperty(ref isGridVisible, value);
        }

        private bool tapFeed;
        public bool TapFeed
        {
            get => tapFeed;
            set => SetProperty(ref tapFeed, value);
        }

        public SingleExecutionCommand OpenDeleteMenuCommand => SingleExecutionCommand.FromFunc(OnOpenDeleteMenu);

        public SingleExecutionCommand CloseDeleteMenuCommand => SingleExecutionCommand.FromFunc(OnCloseDeleteMenu);

        public SingleExecutionCommand DeleteBookmarksCommand => SingleExecutionCommand.FromFunc(OnDeleteBookmarks);

        #endregion

        #region -- BaseFeedPageViewModel implement --

        protected override async Task<List<Tweet>> GetPartOfTweets(Guid userId, DateTime timePoint, TimeDirection direction)
        {
            var result = await FeedService.GetTweetsMarkedUser(userId, timePoint, direction);
            return result.Result;
        }

        #endregion

        #region -- Overrides --

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsDeleteMenuVisible = false;

            if (Tweets.Count == 0)
            {
                await AddTweets(await GetPartOfTweets(AuthorizationService.CurrentUserId, DateTime.Now, TimeDirection.Earlier), TimeDirection.Earlier);
                ChangeElementsVisible(Tweets.Count > 0);
            }
            else
            {
                ChangeElementsVisible(true);
                List<Tweet> tweetslist = null;
                AOResult<List<Tweet>> getList;

                if (Tweets.Count <= 5)
                {
                    getList = await FeedService.GetTweetsMarkedUser(AuthorizationService.CurrentUserId, DateTime.Now, TimeDirection.Earlier);
                }
                else
                {
                    getList = await FeedService.GetTweetsMarkedUser(AuthorizationService.CurrentUserId, DateTime.Now, TimeDirection.Earlier, Tweets.Count);
                }

                if (getList.IsSuccess)
                {
                    tweetslist = getList.Result;

                    if (tweetslist != null)
                    {
                        await UpdateTweets(tweetslist);
                    }
                }
            }

            ChangeElementsVisible(Tweets.Count > 0);
        }

        protected override async Task MarkTappedHandler(TweetViewModel tweet)
        {
            await base.MarkTappedHandler(tweet);
            int index = Tweets.IndexOf(tweet);

            if (index != -1)
            {
                Tweets.RemoveAt(index);
            }

            ChangeElementsVisible(Tweets.Count > 0);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(TapFeed))
            {
                if (TapFeed)
                {
                    OnCloseDeleteMenu();
                    TapFeed = false;
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private Task OnOpenDeleteMenu()
        {
            IsDeleteMenuVisible = true;
            return Task.CompletedTask;
        }

        private Task OnCloseDeleteMenu()
        {
            IsDeleteMenuVisible = false;
            return Task.CompletedTask;
        }

        private async Task OnDeleteBookmarks()
        {
            IsDeleteMenuVisible = false;

            var isDeleteBookmarks = await _pageDialog.DisplayAlertAsync(
                title: TextResources["Question"],
                message: TextResources["DeleteAllBookmarksMessage"],
                acceptButton: TextResources["Confirm"],
                cancelButton: TextResources["Cancel"]);

            if (isDeleteBookmarks)
            {
                await TweetService.DeleteAllMarkedTweetsAsync(AuthorizationService.CurrentUserId);
                ChangeElementsVisible(false);
                Tweets.Clear();
            }
        }

        private async Task UpdateTweets(List<Tweet> list)
        {
            for (int i = 0; i < Tweets.Count; i++)
            {
                Tweet tweet = list.FirstOrDefault(t => t.Id == Tweets[i].Id);

                if (tweet != null)
                {
                    list.Remove(tweet);
                }
                else
                {
                    Tweets.RemoveAt(i--);
                }
            }

            int counter = 0;

            for (int i = 0; i < Tweets.Count; i++)
            {
                if (counter > list.Count - 1)
                {
                    break;
                }

                if (Tweets[i].CreationTime <= list[counter].CreationTime)
                {
                    Tweets.Insert(i, list[counter].GetViewModel(AuthorizationService.CurrentUserId));
                    counter++;
                }
            }

            if (counter < list.Count)
            {
                for (int i = counter; i < list.Count; i++)
                {
                    Tweets.Add(list[i].GetViewModel(AuthorizationService.CurrentUserId));
                }
            }

            if (Tweets.Count > 0)
            {
                _lowerDateTime = Tweets[Tweets.Count - 1].CreationTime;
            }
        }

        /* change visibility DotsImage and Grid with Bookmarks have no yet label */
        private void ChangeElementsVisible(bool visibility)
        {
            IsDotsImageVisible = visibility;
            IsGridVisible = !visibility;
        }

        #endregion
    }
}