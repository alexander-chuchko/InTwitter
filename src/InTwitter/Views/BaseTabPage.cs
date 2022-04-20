using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace InTwitter.Views
{
    public class BaseTabPage : ContentPage
    {
        public BaseTabPage()
        {
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        #region ---Public properties---

        public static readonly BindableProperty SelectedTabIconProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectedTabIcon),
                returnType: typeof(string),
                declaringType: typeof(BaseTabPage),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string SelectedTabIcon
        {
            get => (string)GetValue(SelectedTabIconProperty);
            set => SetValue(SelectedTabIconProperty, value);
        }

        public static readonly BindableProperty UnselectedTabIconProperty =
            BindableProperty.Create(
                propertyName: nameof(UnselectedTabIcon),
                returnType: typeof(string),
                declaringType: typeof(BaseTabPage),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string UnselectedTabIcon
        {
            get => (string)GetValue(UnselectedTabIconProperty);
            set => SetValue(UnselectedTabIconProperty, value);
        }

        #endregion
    }
}
