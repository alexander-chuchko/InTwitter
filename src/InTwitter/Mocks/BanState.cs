using System.Collections.Generic;
using InTwitter.Models;

namespace InTwitter.Mocks
{
    public class BanState
    {
        #region ---Public Static Properties---

        private static BanState _instance;

        public static BanState Instance => _instance ??= new BanState();

        #endregion

        #region ---Constructors---

        private BanState()
        {
            this.Bans = new List<Ban>();
        }

        #endregion

        #region ---Public Properties---

        public List<Ban> Bans { get; set; }

        #endregion
    }
}
