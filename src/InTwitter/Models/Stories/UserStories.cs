using Realms;
using System;
using System.Linq;

namespace InTwitter.Models.Stories
{
    public class UserStories : RealmObject
    {
        [PrimaryKey]
        public string Id { get; set; }
        public DateTimeOffset LastUpdatedPost { get; set; }

        [Backlink(nameof(MediaStoriesSource.Owner))]
        public IQueryable<MediaStoriesSource> MediaStoriesSources { get; }

        [Backlink(nameof(Stories.PostData.Owner))]
        public IQueryable<PostData> PostData { get; }
    }
}
