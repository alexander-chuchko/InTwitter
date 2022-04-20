using System.Linq;
using InTwitter.Models;
using InTwitter.Mocks;
using InTwitter.ViewModels;
using InTwitter.Services.AuthorizationService;
using InTwitter.Models.Tweet;
using InTwitter.Models.User;
using System;

namespace InTwitter.Extensions
{
    public static class TweetExtension
    {
        public static TweetViewModel GetViewModel(this Tweet tweet, Guid currentUserId)
        {
            TweetViewModel tweetVM = new TweetViewModel()
            {
                Id = tweet.Id,
                Text = tweet.Text,
                CreationTime = tweet.CreationTime,
                Version = tweet.Version,
            };

            tweetVM.User = tweet.Owner.ToUserViewModel();

            tweetVM.MediaSources = MediaSourceState.Instance.Medias.Where(m => m.TweetId == tweet.Id).Select(m => m.GetViewModel()).ToList();

            tweetVM.LikesAmount = LikeState.Instance.Likes.Where(l => l.TweetId == tweet.Id).Count();

            tweetVM.IsUserLiked = LikeState.Instance.Likes.Any(l => l.UserId == currentUserId && l.TweetId == tweet.Id);

            tweetVM.IsUserMarked = MarkState.Instance.Marks.Any(m => m.UserId == currentUserId && m.TweetId == tweet.Id);

            return tweetVM;
        }

        public static Tweet ToTweet(this TweetViewModel tweetVM)
        {
            Tweet tweet = new Tweet()
            {
                Id = tweetVM.Id,
                Text = tweetVM.Text,
                CreationTime = tweetVM.CreationTime,
                UserId = tweetVM.User.Id,
            };

            return tweet;
        }
    }
}
