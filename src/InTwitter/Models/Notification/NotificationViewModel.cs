using InTwitter.Enums;
using InTwitter.Models.Base;
using InTwitter.Models.Tweet;
using InTwitter.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace InTwitter.Models.Notification
{
    public class NotificationViewModel : EntityViewModelBase
    {
        #region -- Public properties --

        private string actionText;
        public string ActionText
        {
            get => actionText;
            set => SetProperty(ref actionText, value);
        }

        private TweetViewModel tweet;
        public TweetViewModel Tweet
        {
            get => tweet;
            set => SetProperty(ref tweet, value);
        }

        private UserViewModel user;
        public UserViewModel User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private string typeImageAction;
        public string TypeImageAction
        {
            get => typeImageAction;
            set => SetProperty(ref typeImageAction, value);
        }

        private EMediaType mediaType;
        public EMediaType MediaType
        {
            get => mediaType;
            set => SetProperty(ref mediaType, value);
        }

        private string videoText;
        public string VideoText
        {
            get => videoText;
            set => SetProperty(ref videoText, value);
        }

        #endregion

        #region -- Overrides --

        public override Guid Id { get; set; }

        #endregion
    }
}