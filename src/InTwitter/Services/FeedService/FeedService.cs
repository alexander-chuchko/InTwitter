using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InTwitter.Models;
using InTwitter.Models.Tweet;
using InTwitter.Helpers;
using InTwitter.Mocks;
using InTwitter.Services.UserService;
using InTwitter.Models.Notification;
using InTwitter.Models.User;
using InTwitter.Services.AuthorizationService;

namespace InTwitter.Services.FeedService
{
    public class FeedService : IFeedService
    {
        private IUserService _userService;

        public FeedService(IUserService userService)
        {
            _userService = userService;
        }

        #region ---IFeedService Implementation---

        public async Task<AOResult<List<Tweet>>> GetFeedToUser(Guid userId, DateTime time, TimeDirection timeDirection)
        {
            var result = new AOResult<List<Tweet>>();

            List<Tweet> tweets = new List<Tweet>();

            var followers = FollowerState.Instance.Followers.Where(f => f.UserWhoId == userId).Select(f => f.UserOnId).ToList();

            followers.Add(userId);

            foreach (var id in followers)
            {
                tweets.AddRange(await this.GetAllTweetsFromUser(id, time, timeDirection));
            }

            if (timeDirection == TimeDirection.Later)
            {
                tweets = tweets.OrderBy(t => t.CreationTime).ToList();
            }
            else
            {
                tweets = tweets.OrderByDescending(t => t.CreationTime).ToList();
            }

            result.SetSuccess(await AttachUserInstanceToTweet(TweetState.Instance.GetTruncatedList(tweets)));

            return result;
        }

        public async Task<AOResult<List<Tweet>>> GetTweetsBySearch(string search, DateTime time, TimeDirection timeDirection)
        {
            var result = new AOResult<List<Tweet>>();
            List<Tweet> tweets = new List<Tweet>();

            if (search != string.Empty)
            {
                search = search.Trim();
                search = search.ToLower();
                if (timeDirection == TimeDirection.Earlier)
                {
                    tweets = TweetState.Instance.Tweets.Where(t => t.CreationTime < time && t.Text.ToLower().Contains(search)).OrderByDescending(t => t.CreationTime).ToList();
                }
                else
                {
                    tweets = TweetState.Instance.Tweets.Where(t => t.CreationTime > time && t.Text.Contains(search)).OrderBy(t => t.CreationTime).ToList();
                }
            }

            result.SetSuccess(await AttachUserInstanceToTweet(TweetState.Instance.GetTruncatedList(tweets)));

            return result;
        }

        public async Task<AOResult<List<Tweet>>> GetTweetsFromUser(Guid userId, DateTime time, TimeDirection timeDirection)
        {
            var result = new AOResult<List<Tweet>>();
            List<Tweet> tweets = new List<Tweet>();

            tweets = await GetAllTweetsFromUser(userId, time, timeDirection);

            result.SetSuccess(await AttachUserInstanceToTweet(TweetState.Instance.GetTruncatedList(tweets)));

            return result;
        }

        private async Task<List<Tweet>> AttachUserInstanceToTweet(List<Tweet> tweets)
        {
            var userIds = tweets.Select(t => t.UserId).ToHashSet().ToList();

            var users = (await _userService.GetUsersAsync(userIds)).Result;

            foreach (var tweet in tweets)
            {
                tweet.Owner = users.FirstOrDefault(u => u.Id == tweet.UserId);
            }

            return tweets;
        }

        private async Task<List<Tweet>> GetAllTweetsFromUser(Guid userId, DateTime time, TimeDirection timeDirection)
        {
            List<Tweet> result = new List<Tweet>();

            Func<Tweet, bool> predicate;

            if (timeDirection == TimeDirection.Earlier)
            {
                predicate = new Func<Tweet, bool>(t => t.UserId == userId && t.CreationTime < time);
                result = TweetState.Instance.Tweets.Where(predicate).OrderByDescending(t => t.CreationTime).ToList();
            }
            else
            {
                predicate = new Func<Tweet, bool>(t => t.UserId == userId && t.CreationTime > time);
                result = TweetState.Instance.Tweets.Where(predicate).OrderBy(t => t.CreationTime).ToList();
            }

            return result;
        }

        public async Task<AOResult<List<Tweet>>> GetTweetsLikedUser(Guid userId, DateTime time, TimeDirection timeDirection, int amount = 5)
        {
            var result = new AOResult<List<Tweet>>();
            List<Tweet> tweets = new List<Tweet>();

            var likes = LikeState.Instance.Likes.Where(l => l.UserId == userId);

            foreach (var like in likes)
            {
                tweets.Add(TweetState.Instance.Tweets.Where(t => t.Id == like.TweetId).FirstOrDefault());
            }

            if (timeDirection == TimeDirection.Later)
            {
                tweets = tweets.Where(t => t.CreationTime > time).OrderBy(t => t.CreationTime).ToList();
            }
            else
            {
                tweets = tweets.Where(t => t.CreationTime < time).OrderByDescending(t => t.CreationTime).ToList();
            }

            result.SetSuccess(await AttachUserInstanceToTweet(TweetState.Instance.GetTruncatedList(tweets, (uint)amount)));

            return result;
        }

        public async Task<AOResult<List<Tweet>>> GetTweetsMarkedUser(Guid userId, DateTime time, TimeDirection timeDirection, int amount = 5)
        {
            var result = new AOResult<List<Tweet>>();
            List<Tweet> tweets = new List<Tweet>();

            var marks = MarkState.Instance.Marks.Where(m => m.UserId == userId);

            foreach (var mark in marks)
            {
                tweets.Add(TweetState.Instance.Tweets.Where(t => t.Id == mark.TweetId).FirstOrDefault());
            }

            if (timeDirection == TimeDirection.Later)
            {
                tweets = tweets.Where(t => t.CreationTime > time).OrderBy(t => t.CreationTime).ToList();
            }
            else
            {
                tweets = tweets.Where(t => t.CreationTime < time).OrderByDescending(t => t.CreationTime).ToList();
            }

            tweets = TweetState.Instance.GetTruncatedList(tweets, (uint)amount);

            result.SetSuccess(await AttachUserInstanceToTweet(tweets));

            return result;
        }

        public async Task<AOResult<List<Notification>>> GetNotificationsUser(Guid userId, DateTime time, TimeDirection timeDirection, int amount = 5)
        {
            AOResult<List<Notification>> result = new AOResult<List<Notification>>();
            List<Notification> notifications = NotificationState.Instance.Notifications.Where(n => n.OwnerId == userId).ToList();

            if (timeDirection == TimeDirection.Later)
            {
                notifications = notifications.Where(t => t.CreationTime > time).OrderBy(t => t.CreationTime).ToList();
            }
            else
            {
                notifications = notifications.Where(t => t.CreationTime < time).OrderByDescending(t => t.CreationTime).ToList();
            }

            foreach (Notification notification in notifications)
            {
                notification.User = UserState.Instance.Users.Where(u => u.Id == notification.UserId).FirstOrDefault();
                notification.Tweet = TweetState.Instance.Tweets.Where(t => t.Id == notification.TweetId).FirstOrDefault();
                notification.Tweet.Owner = UserState.Instance.Users.FirstOrDefault(u => u.Id == userId);
            }

            notifications = TweetState.Instance.GetTruncatedList(notifications, (uint)amount);
            result.SetSuccess(notifications);

            return result;
        }

        #endregion
    }
}
