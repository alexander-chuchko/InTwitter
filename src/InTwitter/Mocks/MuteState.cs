using System.Collections.Generic;
using InTwitter.Models;

namespace InTwitter.Mocks
{
    public class MuteState
    {
        #region ---Public Static Properties---

        private static MuteState _instance;

        public static MuteState Instance => _instance ??= new MuteState();

        #endregion

        #region ---Constructors---

        private MuteState()
        {
            this.Mutes = new List<Mute>();
        }

        #endregion

        #region ---Public Properties---

        public List<Mute> Mutes { get; set; }

        #endregion
    }
}
