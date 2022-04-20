using System;
using InTwitter.PlatformDependencyInterface;
using Google.Android.Material.Snackbar;
using Xamarin.Forms;
using Xamarin.Essentials;

[assembly: Dependency(typeof(InTwitter.Droid.DependencyInterfaceImplementations.SnackbarShower))]
namespace InTwitter.Droid.DependencyInterfaceImplementations
{
    public class SnackbarShower : ISnackbarShower
    {
        #region ---ISnackbarShower Implementation---

        public void Show(string message, int snackbarLifetime = 3000, string actionTitle = null, Action action = null)
        {
            var content = Platform.CurrentActivity?.Window?.DecorView;

            if (content != null)
            {
                var snack = Snackbar.Make(content.RootView, message, snackbarLifetime);
                if (action != null)
                {
                    snack.SetAction(actionTitle, (a) =>
                    {
                        action();
                    });
                }
                snack.Show();
            }
        }

        #endregion
    }
}