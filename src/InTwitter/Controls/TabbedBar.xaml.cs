using InTwitter.Enums;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public partial class TabbedBar : StackLayout
    {
        public TabbedBar()
        {
            InitializeComponent();
        }

        #region ---Public properties---

        public static readonly BindableProperty TabFontProperty =
            BindableProperty.Create(
                propertyName: nameof(TabFont),
                returnType: typeof(string),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string TabFont
        {
            get => (string)GetValue(TabFontProperty);
            set => SetValue(TabFontProperty, value);
        }

        public static readonly BindableProperty TabFontSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(TabFontSize),
                returnType: typeof(double),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public double TabFontSize
        {
            get => (double)GetValue(TabFontSizeProperty);
            set => SetValue(TabFontSizeProperty, value);
        }

        public static readonly BindableProperty SelectedTabColorProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectedTabColor),
                returnType: typeof(Color),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color SelectedTabColor
        {
            get => (Color)GetValue(SelectedTabColorProperty);
            set => SetValue(SelectedTabColorProperty, value);
        }

        public static readonly BindableProperty FirstTabTextProperty =
            BindableProperty.Create(
                propertyName: nameof(FirstTabText),
                returnType: typeof(string),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string FirstTabText
        {
            get => (string)GetValue(FirstTabTextProperty);
            set => SetValue(FirstTabTextProperty, value);
        }

        public static readonly BindableProperty SecondTabTextProperty =
            BindableProperty.Create(
                propertyName: nameof(SecondTabText),
                returnType: typeof(string),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string SecondTabText
        {
            get => (string)GetValue(SecondTabTextProperty);
            set => SetValue(SecondTabTextProperty, value);
        }

        public static readonly BindableProperty UnselectedTabColorProperty =
            BindableProperty.Create(
                propertyName: nameof(UnselectedTabColor),
                returnType: typeof(Color),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color UnselectedTabColor
        {
            get => (Color)GetValue(UnselectedTabColorProperty);
            set => SetValue(UnselectedTabColorProperty, value);
        }

        public static readonly BindableProperty FirstTabColorProperty =
            BindableProperty.Create(
                propertyName: nameof(FirstTabColor),
                returnType: typeof(Color),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color FirstTabColor
        {
            get => (Color)GetValue(FirstTabColorProperty);
            set => SetValue(FirstTabColorProperty, value);
        }

        public static readonly BindableProperty SecondTabColorProperty =
            BindableProperty.Create(
                propertyName: nameof(SecondTabColor),
                returnType: typeof(Color),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Color SecondTabColor
        {
            get => (Color)GetValue(SecondTabColorProperty);
            set => SetValue(SecondTabColorProperty, value);
        }

        public static readonly BindableProperty SeparatorMagrinProperty =
            BindableProperty.Create(
                propertyName: nameof(SeparatorMagrin),
                returnType: typeof(Thickness),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public Thickness SeparatorMagrin
        {
            get => (Thickness)GetValue(SeparatorMagrinProperty);
            set => SetValue(SeparatorMagrinProperty, value);
        }

        public static readonly BindableProperty SelectedTabProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectedTab),
                returnType: typeof(EProfileTweetsState),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public EProfileTweetsState SelectedTab
        {
            get => (EProfileTweetsState)GetValue(SelectedTabProperty);
            set => SetValue(SelectedTabProperty, value);
        }

        public static readonly BindableProperty SelectFirstTabCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectFirstTabCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand SelectFirstTabCommand
        {
            get => (ICommand)GetValue(SelectFirstTabCommandProperty);
            set => SetValue(SelectFirstTabCommandProperty, value);
        }

        public static readonly BindableProperty SelectSecondTabCommandProperty =
            BindableProperty.Create(
                propertyName: nameof(SelectSecondTabCommand),
                returnType: typeof(ICommand),
                declaringType: typeof(TabbedBar),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand SelectSecondTabCommand
        {
            get => (ICommand)GetValue(SelectSecondTabCommandProperty);
            set => SetValue(SelectSecondTabCommandProperty, value);
        }

        #endregion

        #region ---Overrides---

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedTab))
            {
                if (SelectedTab == EProfileTweetsState.Posts)
                {
                    FirstTabColor = SelectedTabColor;
                    SecondTabColor = UnselectedTabColor;
                    SeparatorMagrin = new Thickness(0);
                }
                else
                {
                    FirstTabColor = UnselectedTabColor;
                    SecondTabColor = SelectedTabColor;
                    SeparatorMagrin = new Thickness(73, 0, 0, 0);
                }
            }
        }

        #endregion
    }
}