using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using InTwitter.PlatformDependencyInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(InTwitter.Droid.DependencyInterfaceImplementations.ChangerStatusBar))]
namespace InTwitter.Droid.DependencyInterfaceImplementations
{
    public class ChangerStatusBar : IChangerStatusBar
    {

        public void ChangeStatusBarColor(Color color)
        {
            Platform.CurrentActivity?.Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor(color.ToHex()));
        }

        public void ChangeTitleColor(bool isLight)
        {
            var element = Platform.CurrentActivity.Window?.DecorView;

            if (element != null)
            {
                if (isLight)
                {
                    element.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
                }
                else
                {
                    element.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
                }

            }
        }
    }
}