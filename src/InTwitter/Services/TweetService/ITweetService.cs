using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTwitter.Models;
using InTwitter.Models.Base;
using InTwitter.Models.Tweet;
using InTwitter.Helpers;

namespace InTwitter.Services.TweetService
{
    public interface ITweetService
    {
        Task CreateTweetAsync(Tweet tweet, List<TweetMediaSource> media);

        Task DeleteTweetAsync(Tweet tweet);

        Task<AOResult<List<Tweet>>> GetUpToDateTweets(List<IVersionController> tweets);

        Task SetIsTweetLikedAsync(Guid userId, Guid tweetId, bool isLiked);

        Task SetIsTweetMarkedAsync(Guid userId, Guid tweetId, bool isMarked);

        Task DeleteAllMarkedTweetsAsync(Guid userId);
    }
}
