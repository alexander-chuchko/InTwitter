using System;
using System.Linq;
using System.Threading.Tasks;
using InTwitter.Models;
using InTwitter.Mocks;
using InTwitter.Helpers;
using System.Collections.Generic;
using InTwitter.Models.Base;
using InTwitter.Models.Tweet;
using InTwitter.Mocks;
using InTwitter.Models.Notification;
using InTwitter.Enums;

namespace InTwitter.Services.TweetService
{
    public class TweetService : ITweetService
    {
        #region ---ITweetService Implementation---

        public async Task CreateTweetAsync(Tweet tweet, List<TweetMediaSource> media)
        {
            TweetState.Instance.Tweets.Add(tweet);
            foreach (var item in media)
            {
                MediaSourceState.Instance.Medias.Add(item);
            }

            AddActions(tweet);
        }

        public async Task DeleteTweetAsync(Tweet tweet)
        {
            TweetState.Instance.Tweets.Remove(tweet);
        }

        public async Task<AOResult<List<Tweet>>> GetUpToDateTweets(List<IVersionController> tweets)
        {
            var result = new AOResult<List<Tweet>>();
            var udtTweets = new List<Tweet>();

            foreach (var tweet in tweets)
            {
                var utdTweet = Mocks.TweetState.Instance.Tweets.FirstOrDefault(t => t.Id == tweet.Id && t.Version != tweet.Version);

                if (utdTweet != null)
                {
                    udtTweets.Add(utdTweet);
                }
            }

            result.SetSuccess(udtTweets);

            return result;
        }

        public async Task SetIsTweetLikedAsync(Guid userId, Guid tweetId, bool isLiked)
        {
            var like = LikeState.Instance.Likes.Where(l => l.TweetId == tweetId && l.UserId == userId).FirstOrDefault();

            if (isLiked == true && like == null)
            {
                LikeState.Instance.Likes.Add(new Like()
                {
                    UserId = userId,
                    TweetId = tweetId,
                });
                TweetState.Instance.IncreaseTweetVersion(tweetId);
            }
            else if (isLiked == false && like != null)
            {
                LikeState.Instance.Likes.Remove(like);
                TweetState.Instance.IncreaseTweetVersion(tweetId);
            }
        }

        public async Task SetIsTweetMarkedAsync(Guid userId, Guid tweetId, bool isMarked)
        {
            var mark = MarkState.Instance.Marks.Where(m => m.TweetId == tweetId && m.UserId == userId).FirstOrDefault();

            if (isMarked == true && mark == null)
            {
                MarkState.Instance.Marks.Add(new Mark()
                {
                    UserId = userId,
                    TweetId = tweetId,
                });
                TweetState.Instance.IncreaseTweetVersion(tweetId);
            }
            else if (isMarked == false && mark != null)
            {
                MarkState.Instance.Marks.Remove(mark);
                TweetState.Instance.IncreaseTweetVersion(tweetId);
            }
        }

        public async Task DeleteAllMarkedTweetsAsync(Guid userId)
        {
            List<Mark> marks = MarkState.Instance.Marks.Where(m => m.UserId == userId).ToList();

            await Task.Run(() =>
            {
                foreach (Mark mark in marks)
                {
                    TweetState.Instance.IncreaseTweetVersion(mark.TweetId);
                    MarkState.Instance.Marks.Remove(mark);
                }
            });
        }

        #endregion

        #region -- Private helpers --

        private void AddActions(Tweet tweet)
        {
            DateTime time = DateTime.Now;
            Random random = new Random();

            for (int k = 0; k < random.Next(0, UserState.Instance.Users.Count); k++)
            {
                LikeState.Instance.Likes.Add(new Like()
                {
                    UserId = UserState.Instance.Users[random.Next(0, UserState.Instance.Users.Count)].Id,
                    TweetId = tweet.Id,
                });

                NotificationState.Instance.Notifications.Add(new Notification
                {
                    Id = Guid.Empty,
                    OwnerId = UserState.Instance.Users[UserState.Instance.Users.Count - 1].Id,
                    UserId = LikeState.Instance.Likes[LikeState.Instance.Likes.Count - 1].UserId,
                    TweetId = tweet.Id,
                    CreationTime = time,
                    TweetAction = ETweetNotificationAction.Like,
                });

                Task.Delay(10);
                time -= new TimeSpan(1, random.Next(-59, 59), 0);
            }

            time = DateTime.Now;

            for (int k = 0; k < random.Next(0, UserState.Instance.Users.Count); k++)
            {
                MarkState.Instance.Marks.Add(new Mark()
                {
                    UserId = UserState.Instance.Users[random.Next(0, UserState.Instance.Users.Count)].Id,
                    TweetId = tweet.Id,
                });

                NotificationState.Instance.Notifications.Add(new Notification
                {
                    Id = Guid.Empty,
                    OwnerId = UserState.Instance.Users[UserState.Instance.Users.Count - 1].Id,
                    UserId = MarkState.Instance.Marks[MarkState.Instance.Marks.Count - 1].UserId,
                    TweetId = tweet.Id,
                    CreationTime = time,
                    TweetAction = ETweetNotificationAction.Mark,
                });

                Task.Delay(10);
                time -= new TimeSpan(1, random.Next(-59, 59), 0);
            }
        }
        #endregion
    }
}