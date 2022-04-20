using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InTwitter.Models;
using InTwitter.Models.Tweet;
using InTwitter.Helpers;
using InTwitter.Models.Notification;

namespace InTwitter.Services.FeedService
{
    public enum TimeDirection
    {
        Later, Earlier
    }

    public interface IFeedService
    {
        Task<AOResult<List<Tweet>>> GetFeedToUser(Guid userId, DateTime time, TimeDirection timeDirection);

        Task<AOResult<List<Tweet>>> GetTweetsFromUser(Guid userId, DateTime time, TimeDirection timeDirection);

        Task<AOResult<List<Tweet>>> GetTweetsBySearch(string search, DateTime time, TimeDirection timeDirection);

        Task<AOResult<List<Tweet>>> GetTweetsLikedUser(Guid userId, DateTime time, TimeDirection timeDirection, int amount = 5);

        Task<AOResult<List<Tweet>>> GetTweetsMarkedUser(Guid userId, DateTime time, TimeDirection timeDirection, int amount = 5);

        Task<AOResult<List<Notification>>> GetNotificationsUser(Guid userId, DateTime time, TimeDirection timeDirection, int amount = 5);
    }
}
