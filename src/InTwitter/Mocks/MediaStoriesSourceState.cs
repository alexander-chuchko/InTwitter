using InTwitter.Models.Stories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InTwitter.Mocks
{
    public class MediaStoriesSourceState
    {
        #region ---Public Static Properties---

        private static MediaStoriesSourceState _instance;

        public static MediaStoriesSourceState Instance => _instance ??= new MediaStoriesSourceState();

        #endregion

        #region ---Constructors---

        private MediaStoriesSourceState()
        {
            this.MediaStoriesSources = new List<MediaStoriesSource>();
        }

        #endregion

        #region ---Public Properties---

        public List<MediaStoriesSource> MediaStoriesSources { get; set; }

        #endregion
    }
}
