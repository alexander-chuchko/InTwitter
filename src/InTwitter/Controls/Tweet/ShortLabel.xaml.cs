using Xamarin.Forms;

namespace InTwitter.Controls
{
    public partial class ShortLabel
    {
        public ShortLabel()
        {
            InitializeComponent();
        }

        public bool IsShorted
        {
            get => (bool)GetValue(IsShortedProperty);
            set => SetValue(IsShortedProperty, value);
        }

        public static BindableProperty IsShortedProperty = BindableProperty.Create(nameof(IsShorted), typeof(bool), typeof(ShortLabel), coerceValue: OnIsShortedProperty);

        public bool HasShorted
        {
            get => (bool)GetValue(HasShortedProperty);
            set => SetValue(HasShortedProperty, value);
        }

        public static BindableProperty HasShortedProperty = BindableProperty.Create(nameof(HasShorted), typeof(bool), typeof(ShortLabel), defaultBindingMode: BindingMode.OneWayToSource);

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(ShortLabel));

        public int MaxSymbols
        {
            get => (int)GetValue(MaxSymbolsProperty);
            set => SetValue(MaxSymbolsProperty, value);
        }

        public static BindableProperty MaxSymbolsProperty = BindableProperty.Create(nameof(MaxSymbols), typeof(int), typeof(ShortLabel), propertyChanged: OnMaxSymbolsPropertyChanged);

        public string MoreText
        {
            get => (string)GetValue(MoreTextProperty);
            set => SetValue(MoreTextProperty, value);
        }

        public static BindableProperty MoreTextProperty = BindableProperty.Create(nameof(MoreText), typeof(string), typeof(ShortLabel));

        public string CurrentText
        {
            get => (string)GetValue(CurrentTextProperty);
            set => SetValue(CurrentTextProperty, value);
        }

        public static BindableProperty CurrentTextProperty = BindableProperty.Create(nameof(CurrentText), typeof(string), typeof(ShortLabel));

        public string CurrentMoreText
        {
            get => (string)GetValue(CurrentMoreTextProperty);
            set => SetValue(CurrentMoreTextProperty, value);
        }

        public static BindableProperty CurrentMoreTextProperty = BindableProperty.Create(nameof(CurrentMoreText), typeof(string), typeof(ShortLabel));

        private static void OnMaxSymbolsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var label = bindable as ShortLabel;
            var value = (int)newValue;
            label.SetTexts(label.IsShorted);
        }

        private static object OnIsShortedProperty(BindableObject bindable, object value)
        {
            var label = bindable as ShortLabel;
            var isShort = (bool)value;

            label.SetTexts(isShort);

            return value;
        }

        private void SetTexts(bool isShorted)
        {
            if (isShorted == true && LabelText.Length > MaxSymbols)
            {
                CurrentText = LabelText?.Substring(0, MaxSymbols == 0 ? LabelText.Length : MaxSymbols);

                CurrentMoreText = MoreText;
                HasShorted = true;
            }
            else
            {
                CurrentText = LabelText;
                CurrentMoreText = string.Empty;
                HasShorted = false;
            }
        }
    }
}