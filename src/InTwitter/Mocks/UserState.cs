using System.Collections.Generic;
using InTwitter.Models.User;

namespace InTwitter.Mocks
{
    public class UserState
    {
        #region ---Public Static Properties---

        private static UserState _instance;

        public static UserState Instance => _instance ??= new UserState();

        #endregion

        #region ---Constructors---

        private UserState()
        {
            this.Users = new List<User>();
        }

        #endregion

        #region ---Public Properties---

        public List<User> Users { get; set; }

        #endregion

    }
}
