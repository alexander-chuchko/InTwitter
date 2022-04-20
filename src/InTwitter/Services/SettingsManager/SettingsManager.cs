using System;
using Xamarin.Essentials;

namespace InTwitter.Services.SettingsManager
{
    public class SettingsManager : ISettingsManager
    {
        #region ---ISettingsManager Implementation---

        public string SessionToken
        {
            get => Preferences.Get(nameof(SessionToken), Guid.Empty.ToString());
            set => Preferences.Set(nameof(SessionToken), value);
        }

        #endregion
    }
}
