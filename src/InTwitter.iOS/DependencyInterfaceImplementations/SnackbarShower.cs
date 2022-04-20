using System;
using InTwitter.PlatformDependencyInterface;
using TTGSnackBar;
using Xamarin.Forms;

[assembly: Dependency(typeof(InTwitter.iOS.DependencyInterfaceImplementations.SnackbarShower))]
namespace InTwitter.iOS.DependencyInterfaceImplementations
{
    public class SnackbarShower : ISnackbarShower
    {
        #region ---ISnackbarShower Implementation---

        public void Show(string message, int snackbarLifetime = 3000, string actionTitle = null, Action action = null)
        {
            var snackbar = new TTGSnackbar(message);

            snackbar.Duration = new TimeSpan(0, 0, 0, 0, snackbarLifetime);

            if (action != null)
            {
                snackbar.ActionText = actionTitle;
                snackbar.ActionBlock = t =>
                {
                    action();
                };
            }

            snackbar.Show();
        }

        #endregion
    }
}