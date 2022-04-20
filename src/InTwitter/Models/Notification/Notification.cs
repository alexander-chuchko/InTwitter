using System;
using InTwitter.Enums;
using InTwitter.Models.Base;

namespace InTwitter.Models.Notification
{
    public class Notification : EntityBase
    {
        public Guid OwnerId { get; set; }

        public Guid UserId { get; set; }

        public User.User User { get; set; }

        public Guid TweetId { get; set; }

        public Tweet.Tweet Tweet { get; set; }

        public DateTime CreationTime { get; set; }

        public ETweetNotificationAction TweetAction { get; set; }
    }
}