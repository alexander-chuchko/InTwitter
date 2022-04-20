using Prism.Commands;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public partial class LikeControl
    {
        public LikeControl()
        {
            InitializeComponent();
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(LikeControl));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(LikeControl));

        public Style NormalStyle
        {
            get => (Style)GetValue(NormalStyleProperty);
            set => SetValue(NormalStyleProperty, value);
        }

        public static BindableProperty NormalStyleProperty = BindableProperty.Create(
            nameof(NormalStyle),
            typeof(Style),
            typeof(LikeControl));

        public Style ActivatedStyle
        {
            get => (Style)GetValue(ActivatedStyleProperty);
            set => SetValue(ActivatedStyleProperty, value);
        }

        public static BindableProperty ActivatedStyleProperty = BindableProperty.Create(
            nameof(ActivatedStyle),
            typeof(Style),
            typeof(LikeControl));

        public bool IsLiked
        {
            get => (bool)GetValue(IsLikedProperty);
            set => SetValue(IsLikedProperty, value);
        }

        public static BindableProperty IsLikedProperty = BindableProperty.Create(
            nameof(IsLiked),
            typeof(bool),
            typeof(LikeControl),
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: OnIsLikedPropertyCoerce);

        public uint LikesAmount
        {
            get => (uint)GetValue(LikesAmountProperty);
            set => SetValue(LikesAmountProperty, value);
        }

        public static BindableProperty LikesAmountProperty = BindableProperty.Create(
            nameof(LikesAmount),
            typeof(uint),
            typeof(LikeControl),
            defaultBindingMode: BindingMode.TwoWay);

        public ImageSource CheckedImageSource
        {
            get => (ImageSource)GetValue(CheckedImageSourceProperty);
            set => SetValue(CheckedImageSourceProperty, value);
        }

        public ImageSource UncheckedImageSource
        {
            get => (ImageSource)GetValue(UncheckedImageSourceProperty);
            set => SetValue(UncheckedImageSourceProperty, value);
        }

        public static BindableProperty CheckedImageSourceProperty = BindableProperty.Create(
            nameof(CheckedImageSource),
            typeof(ImageSource),
            typeof(LikeControl));

        public static BindableProperty UncheckedImageSourceProperty = BindableProperty.Create(
            nameof(UncheckedImageSource),
            typeof(ImageSource),
            typeof(LikeControl));

        private static object OnIsLikedPropertyCoerce(BindableObject bindable, object newValue)
        {
            var control = bindable as LikeControl;
            var value = (bool)newValue;

            if (value == true)
            {
                control.likesAmountLable.Style = control.ActivatedStyle;
            }
            else
            {
                control.likesAmountLable.Style = control.NormalStyle;
            }

            return newValue;
        }
    }
}