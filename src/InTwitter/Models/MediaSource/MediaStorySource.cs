using InTwitter.Enums;
using InTwitter.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace InTwitter.Models.MediaSource
{
    public class MediaStorySource : Base.EntityViewModelBase
    {
        public EMediaType MediaType { get; set; }

        public string MediaSource { get; set; }

        public DateTimeOffset PublicationTime { get; set; }

        private TimeSpan _TimeSincePublication;
        public TimeSpan TimeSincePublication
        {
            get { return _TimeSincePublication; }
            set { SetProperty(ref _TimeSincePublication, value); }
        }

        public double DurationVideo { get; set; }
    }
}
