using System.Collections.Generic;
using InTwitter.Models;
using System;
using System.Windows.Input;
using Prism.Commands;
using InTwitter.Services.TweetService;
using InTwitter.Services.AuthorizationService;
using System.Linq;
using InTwitter.Models.Base;
using InTwitter.Models.User;

namespace InTwitter.Models.Tweet
{
    public class TweetViewModel : EntityViewModelBase, IVersionController
    {
        public override Guid Id { get; set; }

        public UserViewModel User { get; set; }

        private bool _isShortText = true;
        public bool IsShortText
        {
            get => _isShortText;
            set => SetProperty(ref _isShortText, value);
        }

        private bool _isTextShorted = true;
        public bool IsTextShorted
        {
            get => _isTextShorted;
            set => SetProperty(ref _isTextShorted, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public bool HasMedia => MediaSources.Count > 0;

        public List<MediaSourceViewModel> MediaSources { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreationDate => $"{CreationTime.ToShortDateString()}  •  {CreationTime.ToShortTimeString()}";

        private int _likesAmount;
        public int LikesAmount
        {
            get => _likesAmount;
            set => SetProperty(ref _likesAmount, value);
        }

        private bool _isUserLiked;
        public bool IsUserLiked
        {
            get => _isUserLiked;
            set => SetProperty(ref _isUserLiked, value);
        }

        private bool _isUserMarked;
        public bool IsUserMarked
        {
            get => _isUserMarked;
            set => SetProperty(ref _isUserMarked, value);
        }

        public long Version { get; set; }
    }
}
