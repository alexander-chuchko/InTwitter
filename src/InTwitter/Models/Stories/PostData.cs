using Realms;
using System;

namespace InTwitter.Models.Stories
{
    public class PostData : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public int LastMediaElement { get; set; }
        public UserStories Owner { get; set; }
    }
}
