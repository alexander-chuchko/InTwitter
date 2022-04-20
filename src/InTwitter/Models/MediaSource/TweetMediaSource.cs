using InTwitter.Enums;
using System;

namespace InTwitter.Models
{
    public class TweetMediaSource : Base.EntityBase
    {
        public TweetMediaSource()
        {
            Id = Guid.NewGuid();
        }

        public Guid TweetId { get; set; }

        public string MediaSource { get; set; }

        public EMediaType MediaType { get; set; }
    }
}
