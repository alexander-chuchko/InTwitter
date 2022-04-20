using Android.Views.InputMethods;
using InTwitter.Droid.DependencyInterfaceImplementations;
using InTwitter.PlatformDependencyInterface;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidForceKeyboardDismiss))]
namespace InTwitter.Droid.DependencyInterfaceImplementations
{
    public class AndroidForceKeyboardDismiss : IForceKeyboardDismiss
    {
        public void DismissKeyboard()
        {
            InputMethodManager inputMethod = InputMethodManager.FromContext(CrossCurrentActivity.Current.Activity.ApplicationContext);
            inputMethod.HideSoftInputFromWindow(CrossCurrentActivity.Current.Activity.Window.DecorView.WindowToken,
                                                HideSoftInputFlags.NotAlways);
        }
    }
}