using System;
using System.Collections.Generic;
using InTwitter.Models;

namespace InTwitter.Mocks
{
    public class FollowerState
    {
        #region ---Public Static Properties---

        private static FollowerState _instance;

        public static FollowerState Instance => _instance ??= new FollowerState();

        #endregion

        #region ---Constructors---

        private FollowerState()
        {
            this.Followers = new List<Follower>();
        }

        #endregion

        #region ---Public Properties---

        public List<Follower> Followers { get; set; }

        #endregion

    }
}
