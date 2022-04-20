using InTwitter.Enums;
using Prism.Mvvm;
using System;

namespace InTwitter.Models
{
    public class MediaSourceViewModel : Base.EntityViewModelBase
    {
        public Guid TweetId { get; set; }

        public string MediaSource { get; set; }

        public EMediaType MediaType { get; set; }

        private bool _isPlay;
        public bool IsPlay
        {
            get => _isPlay;
            set => SetProperty(ref _isPlay, value);
        }
    }
}
