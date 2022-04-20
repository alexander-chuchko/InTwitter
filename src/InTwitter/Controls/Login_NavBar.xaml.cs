namespace InTwitter.Controls
{
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public partial class Login_NavBar : ContentView
    {
        public Login_NavBar()
        {
            this.InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty IsBackVisibleProperty =
            BindableProperty.Create(
                                    nameof(IsBackVisible),
                                    typeof(bool),
                                    typeof(Login_NavBar),
                                    defaultValue: default(bool),
                                    defaultBindingMode: BindingMode.OneWayToSource);

        public bool IsBackVisible
        {
            get => (bool)GetValue(IsBackVisibleProperty);
            set => SetValue(IsBackVisibleProperty, value);
        }

        public static readonly BindableProperty TitlePageProperty =
            BindableProperty.Create(
                                    nameof(TitlePage),
                                    typeof(string),
                                    typeof(Login_NavBar),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay);

        public string TitlePage
        {
            get => (string)GetValue(TitlePageProperty);
            set => SetValue(TitlePageProperty, value);
        }

        public static readonly BindableProperty TitlePageColorProperty =
            BindableProperty.Create(
                                    nameof(TitlePageColor),
                                    typeof(Color),
                                    typeof(Login_NavBar),
                                    defaultValue: default(Color),
                                    defaultBindingMode: BindingMode.OneWayToSource);

        public Color TitlePageColor
        {
            get => (Color)GetValue(TitlePageColorProperty);
            set => SetValue(TitlePageColorProperty, value);
        }

        public static readonly BindableProperty TitlePageFontSizeProperty =
            BindableProperty.Create(
                                    nameof(TitlePageFontSize),
                                    typeof(double),
                                    typeof(Login_NavBar),
                                    defaultValue: default(double),
                                    defaultBindingMode: BindingMode.OneWayToSource);

        public double TitlePageFontSize
        {
            get => (double)GetValue(TitlePageFontSizeProperty);
            set => SetValue(TitlePageFontSizeProperty, value);
        }

        public static readonly BindableProperty TitlePageFontFamilyProperty =
            BindableProperty.Create(
                                    nameof(TitlePageFontFamily),
                                    typeof(string),
                                    typeof(Login_NavBar),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.OneWayToSource);

        public string TitlePageFontFamily
        {
            get => (string)GetValue(TitlePageFontFamilyProperty);
            set => SetValue(TitlePageFontFamilyProperty, value);
        }

        public static readonly BindableProperty ArrowBackTappedProperty =
            BindableProperty.Create(
                                    nameof(ArrowBackTapped),
                                    typeof(ICommand),
                                    typeof(Login_NavBar),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay); /*,
                                    propertyChanged: ArrowBackTappedPropertyChanged*/
        //private static void ArrowBackTappedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    throw new NotImplementedException();
        //}
        public ICommand ArrowBackTapped
        {
            get => (ICommand)GetValue(ArrowBackTappedProperty);
            set => SetValue(ArrowBackTappedProperty, value);
        }

        #endregion
    }
}