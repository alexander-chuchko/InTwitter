using InTwitter.iOS.DependencyInterfaceImplementations;
using InTwitter.PlatformDependencyInterface;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(IosForceKeyboardDismiss))]
namespace InTwitter.iOS.DependencyInterfaceImplementations
{
    public class IosForceKeyboardDismiss : IForceKeyboardDismiss
    {
        public void DismissKeyboard()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;

                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.View.EndEditing(true);
            });
        }
    }
}