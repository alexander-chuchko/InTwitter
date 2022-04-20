using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InTwitter.Controls
{
    public partial class MainNavBar : ContentView
    {
        public MainNavBar()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty ScrollStateProperty =
                               BindableProperty.Create(
                                                       nameof(ScrollState),
                                                       typeof(double),
                                                       typeof(MainNavBar),
                                                       defaultValue: default,
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ScrollStatePropertyChanged);

        public double ScrollState
        {
            get => (double)GetValue(ScrollStateProperty);
            set => SetValue(ScrollStateProperty, value);
        }

        public static readonly BindableProperty LeftImageProperty =
                               BindableProperty.Create(
                                                       nameof(LeftImage),
                                                       typeof(string),
                                                       typeof(MainNavBar),
                                                       defaultValue: string.Empty,
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: LeftImagePropertyChanged);

        public string LeftImage
        {
            get => (string)GetValue(LeftImageProperty);
            set => SetValue(LeftImageProperty, value);
        }

        public static readonly BindableProperty RightImageProperty =
                       BindableProperty.Create(
                                               nameof(RightImage),
                                               typeof(string),
                                               typeof(MainNavBar),
                                               defaultValue: string.Empty,
                                               defaultBindingMode: BindingMode.TwoWay,
                                               propertyChanged: RightImagePropertyChanged);

        public string RightImage
        {
            get => (string)GetValue(RightImageProperty);
            set => SetValue(RightImageProperty, value);
        }

        public static readonly BindableProperty TitleProperty =
                               BindableProperty.Create(
                                                       nameof(Title),
                                                       typeof(string),
                                                       typeof(MainNavBar),
                                                       defaultValue: "pic_profile_small.png",
                                                       defaultBindingMode: BindingMode.OneWay);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty LeftImageTapCommandProperty =
                               BindableProperty.Create(
                                                       nameof(LeftImageTapCommand),
                                                       typeof(ICommand),
                                                       typeof(MainNavBar),
                                                       defaultValue: default(ICommand),
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: LeftImageTapCommandPropertyChanged);

        public ICommand LeftImageTapCommand
        {
            get => (ICommand)GetValue(LeftImageTapCommandProperty);
            set => SetValue(LeftImageTapCommandProperty, value);
        }

        public static readonly BindableProperty RightImageTapCommandProperty =
                               BindableProperty.Create(
                                                       nameof(RightImageTapCommand),
                                                       typeof(ICommand),
                                                       typeof(MainNavBar),
                                                       defaultValue: default(ICommand),
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: RightImageTapCommandPropertyChanged);

        public ICommand RightImageTapCommand
        {
            get => (ICommand)GetValue(RightImageTapCommandProperty);
            set => SetValue(RightImageTapCommandProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private static void ScrollStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MainNavBar navBar = bindable as MainNavBar;

            if (navBar != null)
            {
                if ((double)newValue >= 0)
                {
                    double offset = navBar.TranslationY + (double)oldValue - (double)newValue;

                    offset = Math.Min(offset, 0);
                    offset = Math.Max(offset, -navBar.Height);
                    navBar.TranslationY = offset;
                }
            }
        }

        private static void LeftImageTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MainNavBar navBar = bindable as MainNavBar;

            if (navBar != null)
            {
                navBar.leftClickableView.Command = (ICommand)newValue;
            }
        }

        private static void RightImageTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MainNavBar navBar = bindable as MainNavBar;

            if (navBar != null)
            {
                navBar.rightClickableView.Command = (ICommand)newValue;
            }
        }

        private static void LeftImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MainNavBar custom = bindable as MainNavBar;
            if (custom != null)
            {
                custom.leftButton.Source = (string)newValue;
            }
        }

        private static void RightImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            MainNavBar custom = bindable as MainNavBar;
            if (custom != null)
            {
                custom.rightButton.Source = (string)newValue;
            }
        }

        #endregion
    }
}