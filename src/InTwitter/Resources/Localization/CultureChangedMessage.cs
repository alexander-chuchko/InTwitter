using System.Globalization;

namespace InTwitter.Resources.Localization
{
    public class CultureChangedMessage
    {
        #region ---Constructors---

        public CultureChangedMessage(CultureInfo cultureInfo) => NewCultureInfo = cultureInfo;

        #endregion

        #region ---Public Properties---

        public CultureInfo NewCultureInfo { get; set; }

        #endregion

    }
}
