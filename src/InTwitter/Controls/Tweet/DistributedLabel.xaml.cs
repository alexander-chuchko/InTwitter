using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public partial class DistributedLabel
    {
        public DistributedLabel()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(DistributedLabel),
            propertyChanged: OnTextPropertyChanged);

        public double LineHeight
        {
            get => (double)GetValue(LineHeightProperty);
            set => SetValue(LineHeightProperty, value);
        }

        public static BindableProperty LineHeightProperty = BindableProperty.Create(
            nameof(LineHeight),
            typeof(double),
            typeof(DistributedLabel),
            defaultValue: 1.0);

        public Style TextStyle
        {
            get => (Style)GetValue(TextStyleProperty);
            set => SetValue(TextStyleProperty, value);
        }

        public static BindableProperty TextStyleProperty = BindableProperty.Create(
            nameof(TextStyle),
            typeof(Style),
            typeof(DistributedLabel));

        public Style MoreTextStyle
        {
            get => (Style)GetValue(MoreTextStyleProperty);
            set => SetValue(MoreTextStyleProperty, value);
        }

        public static BindableProperty MoreTextStyleProperty = BindableProperty.Create(
            nameof(MoreTextStyle),
            typeof(Style),
            typeof(DistributedLabel));

        public ICommand MoreTapCommand
        {
            get => (ICommand)GetValue(MoreTapCommandProperty);
            set => SetValue(MoreTapCommandProperty, value);
        }

        public static BindableProperty MoreTapCommandProperty = BindableProperty.Create(
            nameof(MoreTapCommand),
            typeof(ICommand),
            typeof(DistributedLabel));

        public bool IsShorted
        {
            get => (bool)GetValue(IsShortedProperty);
            set => SetValue(IsShortedProperty, value);
        }

        public static BindableProperty IsShortedProperty = BindableProperty.Create(nameof(IsShorted), typeof(bool), typeof(DistributedLabel), propertyChanged: OnIsShortedProperty);

        public bool HasShorted
        {
            get => (bool)GetValue(HasShortedProperty);
            set => SetValue(HasShortedProperty, value);
        }

        public static BindableProperty HasShortedProperty = BindableProperty.Create(nameof(HasShorted), typeof(bool), typeof(DistributedLabel), defaultBindingMode: BindingMode.OneWayToSource);

        public int MaxSymbols
        {
            get => (int)GetValue(MaxSymbolsProperty);
            set => SetValue(MaxSymbolsProperty, value);
        }

        public static BindableProperty MaxSymbolsProperty = BindableProperty.Create(nameof(MaxSymbols), typeof(int), typeof(DistributedLabel), propertyChanged: OnMaxSymbolsPropertyChanged);

        private static void OnMaxSymbolsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var label = bindable as DistributedLabel;
            var value = (int)newValue;
            label.UpdateText(label.Text);
        }

        private static void OnIsShortedProperty(BindableObject bindable, object oldVlue, object newValue)
        {
            var label = bindable as DistributedLabel;
            label.UpdateText(label.Text);
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var label = bindable as DistributedLabel;
            var text = (string)newValue;

            if (newValue != null)
            {
                label.UpdateText(text);
            }
        }

        protected virtual void UpdateText(string text)
        {
            if (Text == null)
            {
                return;
            }

            if (IsShorted == true && Text.Length > MaxSymbols)
            {
                text = Text?.Substring(0, MaxSymbols == 0 ? Text.Length : MaxSymbols);
                HasShorted = true;
            }
            else
            {
                text = Text;
                HasShorted = false;
            }

            this.Children.Clear();

            var words = text.Split(' ');

            foreach (var word in words)
            {
                var label = GetSimpleLabel(word + " ");

                this.Children.Add(label);
            }

            if (HasShorted)
            {
                this.Children.Add(GetSimpleLabel("..."));
                this.Children.Add(GetMoreTextLabel("more"));
            }
        }

        protected Label GetSimpleLabel(string word)
        {
            var label = new Label()
            {
                Text = $"{word}",
                Style = TextStyle,
            };

            label.Padding = new Thickness(
                label.Padding.Left,
                label.Padding.Top,
                label.Padding.Right,
                Math.Round(label.FontSize * (LineHeight - 1)));

            return label;
        }

        protected Label GetMoreTextLabel(string word)
        {
            var label = GetSimpleLabel(word);

            label.Style = MoreTextStyle;

            var tapRecognizer = new TapGestureRecognizer()
            {
                Command = MoreTapCommand,
                CommandParameter = BindingContext,
            };

            label.GestureRecognizers.Add(tapRecognizer);

            return label;
        }
    }
}