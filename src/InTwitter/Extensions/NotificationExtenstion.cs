using InTwitter.Models.Notification;
using InTwitter.Resources.Localization;
using System;

namespace InTwitter.Extensions
{
    public static class NotificationExtenstion
    {
        public static NotificationViewModel GetViewModel(this Notification notification, Guid currentUserId, LocalizedResources localized)
        {
            NotificationViewModel notificationViewModel = new NotificationViewModel
            {
                Id = notification.Id,
                MediaType = Enums.EMediaType.None,
                VideoText = string.Empty,
            };

            switch (notification.TweetAction)
            {
                case Enums.ETweetNotificationAction.Mark:
                    notificationViewModel.TypeImageAction = "ic_bookmarks_blue.png";
                    notificationViewModel.ActionText = localized["SavedPost"];
                    break;
                case Enums.ETweetNotificationAction.Like:
                    notificationViewModel.TypeImageAction = "ic_like_blue.png";
                    notificationViewModel.ActionText = localized["LikedPost"];
                    break;
            }

            notificationViewModel.Tweet = notification.Tweet.GetViewModel(currentUserId);
            notificationViewModel.User = notification.User.ToUserViewModel();

            if (notificationViewModel.Tweet.HasMedia)
            {
                notificationViewModel.MediaType = notificationViewModel.Tweet.MediaSources[0].MediaType;

                if (notificationViewModel.MediaType == Enums.EMediaType.Video)
                {
                    notificationViewModel.VideoText = localized["Video"];
                }
            }

            return notificationViewModel;
        }
    }
}