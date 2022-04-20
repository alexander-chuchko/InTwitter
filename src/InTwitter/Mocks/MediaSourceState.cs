using System.Collections.Generic;
using InTwitter.Models;

namespace InTwitter.Mocks
{
    public class MediaSourceState
    {
        private static MediaSourceState _instance;

        public static MediaSourceState Instance => _instance ??= new MediaSourceState();

        private MediaSourceState()
        {
            this.Medias = new List<TweetMediaSource>();
        }

        #region ---Public Properties---

        public List<TweetMediaSource> Medias { get; set; }

        #endregion

    }
}
