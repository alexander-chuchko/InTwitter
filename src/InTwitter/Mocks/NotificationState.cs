using System.Collections.Generic;
using InTwitter.Models.Notification;

namespace InTwitter.Mocks
{
    public class NotificationState
    {
        #region -- Public static properties --

        private static NotificationState _instance;
        public static NotificationState Instance => _instance ??= new NotificationState();

        #endregion

        private NotificationState()
        {
            Notifications = new List<Notification>();
        }

        #region -- Public properties --

        public List<Notification> Notifications { get; set; }

        #endregion
    }
}