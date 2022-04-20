using InTwitter.Models.Stories;
using System.Collections.Generic;

namespace InTwitter.Mocks
{
    public class UserStoriesState
    {
        #region ---Public Static Properties---

        private static UserStoriesState _instance;

        public static UserStoriesState Instance => _instance ??= new UserStoriesState();

        #endregion

        #region ---Constructors---

        private UserStoriesState()
        {
            this.UserStories = new List<UserStories>();
        }

        #endregion

        #region ---Public Properties---

        public List<UserStories> UserStories { get; set; }

        #endregion
    }
}
