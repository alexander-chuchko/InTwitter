using Realms;
using System;

namespace InTwitter.Models.Stories
{
    public class MediaStoriesSource : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public bool IsVideo { get; set; }
        public string MediaSource { get; set; }
        public UserStories Owner { get; set; }
        public DateTimeOffset PublicationTime { get; set; }
    }
}
