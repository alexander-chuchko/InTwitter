using System;

namespace InTwitter.PlatformDependencyInterface
{
    public interface ISnackbarShower
    {
        void Show(string message, int snackbarLifetime = 3000, string actionTitle = null, Action action = null);
    }
}
