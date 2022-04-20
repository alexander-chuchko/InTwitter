using InTwitter.Models.Base;
using System;
using InTwitter.Models.User;

namespace InTwitter.Models.Tweet
{
    public class Tweet : EntityBase, IVersionController
    {
        public Guid UserId { get; set; }

        public User.User Owner { get; set; }

        public string Text { get; set; }

        public DateTime CreationTime { get; set; }

        public long Version { get; set; }
    }
}
