using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InTwitter.Controls.FloatingEntry
{
    public partial class CustomFloatingEntry : ContentView
    {
        private readonly int _topMargin = -25;
        private bool _passwordMode;
        private Color _normalColorOfBoxView;
        private bool _needFocus;

        public CustomFloatingEntry()
        {
            this.InitializeComponent();

            this._passwordMode = false;
            _needFocus = false;
        }

        #region -- Public properties --

        public static readonly BindableProperty EntryFocusedProperty =
           BindableProperty.Create(
                                   nameof(EntryFocused),
                                   typeof(bool),
                                   typeof(CustomFloatingEntry),
                                   defaultValue: false,
                                   defaultBindingMode: BindingMode.TwoWay,
                                   propertyChanged: EntryFocusedPropertyChanged);

        public bool EntryFocused
        {
            get => (bool)GetValue(EntryFocusedProperty);
            set => SetValue(EntryFocusedProperty, value);
        }

        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(
                                    nameof(EntryText),
                                    typeof(string),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: TextPropertyChanged);

        public string EntryText
        {
            get => (string)this.GetValue(EntryTextProperty);
            set => this.SetValue(EntryTextProperty, value);
        }

        public static readonly BindableProperty EntryFontSizeProperty =
            BindableProperty.Create(
                                    nameof(EntryFontSize),
                                    typeof(double),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: default(double),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    coerceValue: EntryFontSizeCoerse);

        public double EntryFontSize
        {
            get => (double)this.GetValue(EntryFontSizeProperty);
            set => this.SetValue(EntryFontSizeProperty, value);
        }

        public static readonly BindableProperty EntryTextColorProperty =
           BindableProperty.Create(
                                   nameof(EntryTextColor),
                                   typeof(Color),
                                   typeof(CustomFloatingEntry),
                                   defaultValue: default(Color),
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color EntryTextColor
        {
            get => (Color)this.GetValue(EntryTextColorProperty);
            set => this.SetValue(EntryTextColorProperty, value);
        }

        public static readonly BindableProperty EntryFontFamilyProperty =
           BindableProperty.Create(
                                   nameof(EntryFontFamily),
                                   typeof(string),
                                   typeof(CustomFloatingEntry),
                                   defaultValue: string.Empty,
                                   defaultBindingMode: BindingMode.TwoWay);

        public string EntryFontFamily
        {
            get => (string)this.GetValue(EntryFontFamilyProperty);
            set => this.SetValue(EntryFontFamilyProperty, value);
        }

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(
                                    nameof(IsPassword),
                                    typeof(bool),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: false,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: IsPasswordPropertyChanged);

        public bool IsPassword
        {
            get => (bool)this.GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public static readonly BindableProperty IsClearImageEnabledProperty =
            BindableProperty.Create(
                                    nameof(IsClearImageEnabled),
                                    typeof(bool),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: true,
                                    defaultBindingMode: BindingMode.TwoWay);

        public bool IsClearImageEnabled
        {
            get => (bool)this.GetValue(IsClearImageEnabledProperty);
            set => this.SetValue(IsClearImageEnabledProperty, value);
        }

        public static readonly BindableProperty LabelTextProperty =
           BindableProperty.Create(
                                   nameof(LabelText),
                                   typeof(string),
                                   typeof(CustomFloatingEntry),
                                   defaultValue: string.Empty,
                                   defaultBindingMode: BindingMode.TwoWay);

        public string LabelText
        {
            get => (string)this.GetValue(LabelTextProperty);
            set => this.SetValue(LabelTextProperty, value);
        }

        public static readonly BindableProperty LabelFontSizeProperty =
            BindableProperty.Create(
                                    nameof(LabelFontSize),
                                    typeof(double),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: default(double),
                                    defaultBindingMode: BindingMode.TwoWay);

        public double LabelFontSize
        {
            get => (double)this.GetValue(LabelFontSizeProperty);
            set => this.SetValue(LabelFontSizeProperty, value);
        }

        public static readonly BindableProperty LabelFontFamilyProperty =
            BindableProperty.Create(
                                    nameof(LabelFontFamily),
                                    typeof(string),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: "Ubuntu500",
                                    defaultBindingMode: BindingMode.TwoWay);

        public string LabelFontFamily
        {
            get => (string)this.GetValue(LabelFontFamilyProperty);
            set => this.SetValue(LabelFontFamilyProperty, value);
        }

        public static readonly BindableProperty PlaceholderColorProperty =
           BindableProperty.Create(
                                   nameof(PlaceholderColor),
                                   typeof(Color),
                                   typeof(CustomFloatingEntry),
                                   defaultValue: default(Color),
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color PlaceholderColor
        {
            get => (Color)this.GetValue(PlaceholderColorProperty);
            set => this.SetValue(PlaceholderColorProperty, value);
        }

        public static readonly BindableProperty LabelTextColorProperty =
           BindableProperty.Create(
                                   nameof(LabelTextColor),
                                   typeof(Color),
                                   typeof(CustomFloatingEntry),
                                   defaultValue: default(Color),
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color LabelTextColor
        {
            get => (Color)this.GetValue(LabelTextColorProperty);
            set => this.SetValue(LabelTextColorProperty, value);
        }

        public static readonly BindableProperty BoxViewColorProperty =
           BindableProperty.Create(
                                   nameof(BoxViewColor),
                                   typeof(Color),
                                   typeof(CustomFloatingEntry),
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color BoxViewColor
        {
            get => (Color)this.GetValue(BoxViewColorProperty);
            set => this.SetValue(BoxViewColorProperty, value);
        }

        public static readonly BindableProperty ErrorTextProperty =
           BindableProperty.Create(
                                   nameof(ErrorText),
                                   typeof(string),
                                   typeof(CustomFloatingEntry),
                                   defaultValue: string.Empty,
                                   defaultBindingMode: BindingMode.TwoWay,
                                   propertyChanged: ErrorTextPropertyChanged);

        public string ErrorText
        {
            get => (string)this.GetValue(ErrorTextProperty);
            set => this.SetValue(ErrorTextProperty, value);
        }

        public static readonly BindableProperty ErrorFontSizeProperty =
            BindableProperty.Create(
                                    nameof(ErrorFontSize),
                                    typeof(double),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: default(double),
                                    defaultBindingMode: BindingMode.TwoWay);

        public double ErrorFontSize
        {
            get => (double)this.GetValue(ErrorFontSizeProperty);
            set => this.SetValue(ErrorFontSizeProperty, value);
        }

        public static readonly BindableProperty ErrorFontFamilyProperty =
            BindableProperty.Create(
                                    nameof(ErrorFontFamily),
                                    typeof(string),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: "Ubuntu500",
                                    defaultBindingMode: BindingMode.TwoWay);

        public string ErrorFontFamily
        {
            get => (string)this.GetValue(ErrorFontFamilyProperty);
            set => this.SetValue(ErrorFontFamilyProperty, value);
        }

        public static readonly BindableProperty ErrorTextColorProperty =
           BindableProperty.Create(
                                   nameof(ErrorTextColor),
                                   typeof(Color),
                                   typeof(CustomFloatingEntry),
                                   defaultBindingMode: BindingMode.TwoWay);

        public Color ErrorTextColor
        {
            get => (Color)this.GetValue(ErrorTextColorProperty);
            set => this.SetValue(ErrorTextColorProperty, value);
        }

        public static readonly BindableProperty LabelRealFontSizeProperty =
            BindableProperty.Create(
                                    nameof(LabelRealFontSize),
                                    typeof(double),
                                    typeof(CustomFloatingEntry),
                                    defaultValue: default(double),
                                    defaultBindingMode: BindingMode.TwoWay);

        public double LabelRealFontSize
        {
            get => (double)this.GetValue(LabelRealFontSizeProperty);
            set => this.SetValue(LabelRealFontSizeProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private static async void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFloatingEntry customFloatingEntry = bindable as CustomFloatingEntry;

            if (customFloatingEntry != null)
            {
                if (!string.IsNullOrWhiteSpace((string)newValue))
                {
                    if (customFloatingEntry.IsClearImageEnabled)
                    {
                        customFloatingEntry.clearImage.IsVisible = true;
                    }

                    if (customFloatingEntry._passwordMode)
                    {
                        customFloatingEntry.eyeImage.IsVisible = true;
                    }
                }
                else
                {
                    if (customFloatingEntry.IsClearImageEnabled)
                    {
                        customFloatingEntry.clearImage.IsVisible = false;
                    }

                    if (customFloatingEntry._passwordMode)
                    {
                        customFloatingEntry.eyeImage.IsVisible = false;
                    }
                }

                if (!customFloatingEntry.entry.IsFocused)
                {
                    if (!string.IsNullOrEmpty((string)newValue))
                    {
                        await customFloatingEntry.TransitionToTitle(false);
                    }
                    else
                    {
                        await customFloatingEntry.TransitionToPlaceholder(false);
                    }
                }
            }
        }

        private static void EntryFocusedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFloatingEntry customFloatingEntry = bindable as CustomFloatingEntry;

            if (customFloatingEntry != null)
            {
                if ((bool)newValue)
                {
                    customFloatingEntry.entry.Focus();
                }
                else
                {
                    customFloatingEntry.entry.Unfocus();
                }
            }
        }

        private static void IsPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFloatingEntry customFloatingEntry = bindable as CustomFloatingEntry;

            if (customFloatingEntry != null)
            {
                if ((bool)newValue)
                {
                    customFloatingEntry._passwordMode = true;
                }
            }
        }

        private static void ErrorTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomFloatingEntry customFloatingEntry = bindable as CustomFloatingEntry;

            if (customFloatingEntry != null)
            {
                if (!string.IsNullOrWhiteSpace((string)newValue))
                {
                    customFloatingEntry._normalColorOfBoxView = customFloatingEntry.BoxViewColor;
                    customFloatingEntry.BoxViewColor = customFloatingEntry.ErrorTextColor;
                    customFloatingEntry.errorLabel.Margin = new Thickness(customFloatingEntry.label.Margin.Left, 0, 0, 0);
                }
                else
                {
                    customFloatingEntry.BoxViewColor = customFloatingEntry._normalColorOfBoxView;
                }
            }
        }

        private static async Task<object> EntryFontSizeCoerse(BindableObject bindable, object value)
        {
            CustomFloatingEntry customFloatingEntry = bindable as CustomFloatingEntry;
            if (customFloatingEntry.entry.FontSize != (double)value)
            {
                customFloatingEntry.entry.FontSize = (double)value;
                await customFloatingEntry.TransitionToPlaceholder(false);
            }

            return value;
        }

        private async void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            if (_needFocus)
            {
                _needFocus = false;
                entry.Focus();
                return;
            }

            await Task.Delay(30);

            if (string.IsNullOrWhiteSpace(EntryText))
            {
                await TransitionToPlaceholder(true);
            }

            EntryFocused = false;
        }

        private async void Entry_Focused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryText))
            {
                await TransitionToTitle(true);
            }

            EntryFocused = true;
            _needFocus = false;
        }

        private async Task TransitionToTitle(bool animated)
        {
            LabelRealFontSize = LabelFontSize;

            if (animated)
            {
                Task<bool> t1 = label.TranslateTo(this.label.Margin.Left, this._topMargin, 100);
                var t2 = SizeTo(LabelRealFontSize);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                label.TranslationX = label.Margin.Left;
                label.TranslationY = _topMargin;
                label.FontSize = LabelRealFontSize;
            }

            label.TextColor = LabelTextColor;
        }

        private async Task TransitionToPlaceholder(bool animated)
        {
            LabelRealFontSize = entry.FontSize;

            if (animated)
            {
                Task<bool> t1 = label.TranslateTo(label.Margin.Left, 0, 100);
                Task t2 = SizeTo(LabelRealFontSize);
                await Task.WhenAll(t1, t2);
            }
            else
            {
                label.TranslationX = label.Margin.Left;
                label.TranslationY = 0;
                label.FontSize = LabelRealFontSize;
            }

            label.TextColor = PlaceholderColor;
        }

        private Task SizeTo(double fontSize)
        {
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            Action<double> callback = input => { label.FontSize = input; };
            double startingHeight = label.FontSize;
            double endingHeight = fontSize;
            uint rate = 5;
            uint length = 100;
            Easing easing = Easing.Linear;

            label.Animate(
                          "invis",
                          callback,
                          startingHeight,
                          endingHeight,
                          rate,
                          length,
                          easing,
                          (v, c) => taskCompletionSource.SetResult(c));

            return taskCompletionSource.Task;
        }

        private void Label_Tapped(object sender, EventArgs e)
        {
            entry.Focus();
            EntryFocused = true;
        }

        private void ClearImage_Tapped(object sender, EventArgs e)
        {
            EntryText = string.Empty;
            _needFocus = true;
        }

        private void EyeImage_Tapped(object sender, EventArgs e)
        {
            if (_passwordMode)
            {
                IsPassword = !IsPassword;
                eyeImage.Source = IsPassword ? "ic_eye_blue.png" : "ic_eye_gray.png";
                _needFocus = true;
            }
        }

        #endregion
    }
}