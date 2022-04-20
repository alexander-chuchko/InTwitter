using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public partial class CheckedButton : Frame
    {
        public CheckedButton()
        {
            InitializeComponent();
        }

        #region ---Public properties---

        public static readonly BindableProperty IconProperty =
           BindableProperty.Create(
                                   nameof(Icon),
                                   typeof(string),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly BindableProperty CheckedIconProperty =
           BindableProperty.Create(
                                   nameof(CheckedIcon),
                                   typeof(string),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public string CheckedIcon
        {
            get => (string)GetValue(CheckedIconProperty);
            set => SetValue(CheckedIconProperty, value);
        }

        public static readonly BindableProperty UncheckedIconProperty =
           BindableProperty.Create(
                                   nameof(UncheckedIcon),
                                   typeof(string),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public string UncheckedIcon
        {
            get => (string)GetValue(UncheckedIconProperty);
            set => SetValue(UncheckedIconProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty =
           BindableProperty.Create(
                                   nameof(FontSize),
                                   typeof(double),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty =
           BindableProperty.Create(
                                   nameof(FontFamily),
                                   typeof(string),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(
                                   nameof(TextColor),
                                   typeof(Color),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty CheckedTextColorProperty =
           BindableProperty.Create(
                                   nameof(CheckedTextColor),
                                   typeof(Color),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color CheckedTextColor
        {
            get => (Color)GetValue(CheckedTextColorProperty);
            set => SetValue(CheckedTextColorProperty, value);
        }

        public static readonly BindableProperty UncheckedTextColorProperty =
           BindableProperty.Create(
                                   nameof(UncheckedTextColor),
                                   typeof(Color),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color UncheckedTextColor
        {
            get => (Color)GetValue(UncheckedTextColorProperty);
            set => SetValue(UncheckedTextColorProperty, value);
        }

        public static readonly BindableProperty TextProperty =
           BindableProperty.Create(
                                   nameof(Text),
                                   typeof(string),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty IsCheckedProperty =
           BindableProperty.Create(
                                   nameof(IsChecked),
                                   typeof(bool),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly BindableProperty TapCommandProperty =
           BindableProperty.Create(
                                   nameof(TapCommand),
                                   typeof(ICommand),
                                   typeof(CheckedButton),
                                   defaultValue: default,
                                   defaultBindingMode: BindingMode.TwoWay);

        public ICommand TapCommand
        {
            get => (ICommand)GetValue(TapCommandProperty);
            set => SetValue(TapCommandProperty, value);
        }

        #endregion

        #region ---Overrides---

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsChecked))
            {
                if (IsChecked)
                {
                    Icon = CheckedIcon;
                    TextColor = CheckedTextColor;

                    foreach (var control in (Parent as Layout).Children)
                    {
                        if (control is CheckedButton)
                        {
                            if (control != this)
                            {
                                ((CheckedButton)control).IsChecked = false;
                            }
                        }
                    }
                }
                else
                {
                    Icon = UncheckedIcon;
                    TextColor = UncheckedTextColor;
                }
            }
        }

        #endregion
    }
}