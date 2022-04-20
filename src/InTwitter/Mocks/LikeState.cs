using System;
using System.Collections.Generic;
using InTwitter.Models;

namespace InTwitter.Mocks
{
    public class LikeState
    {
        #region ---Public Static Properties---

        private static LikeState _instance;

        public static LikeState Instance => _instance ??= new LikeState();

        #endregion

        #region ---Constructors---

        private LikeState()
        {
            this.Likes = new List<Like>();
        }

        #endregion

        #region ---Public Properties---

        public List<Like> Likes { get; set; }

        #endregion
    }
}
