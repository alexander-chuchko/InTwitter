using System;

namespace InTwitter.Models
{
    public class Like
    {
        public Guid UserId { get; set; }

        public Guid TweetId { get; set; }
    }
}
