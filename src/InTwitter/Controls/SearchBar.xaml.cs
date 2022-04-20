using InTwitter.Enums;
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
    public partial class SearchBar
    {
        public SearchBar()
        {
            InitializeComponent();
        }

        public ESearchBarStates State
        {
            get => (ESearchBarStates)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public static BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(ESearchBarStates), typeof(SearchBar));

        public ICommand UserTapCommand
        {
            get => (ICommand)GetValue(UserTapCommandProperty);
            set => SetValue(UserTapCommandProperty, value);
        }

        public static BindableProperty UserTapCommandProperty = BindableProperty.Create(nameof(UserTapCommand), typeof(ICommand), typeof(SearchBar));

        public ICommand SearchCompleteCommand
        {
            get => (ICommand)GetValue(SearchCompleteCommandProperty);
            set => SetValue(SearchCompleteCommandProperty, value);
        }

        public static BindableProperty SearchCompleteCommandProperty = BindableProperty.Create(nameof(SearchCompleteCommand), typeof(ICommand), typeof(SearchBar));

        public string UserIcon
        {
            get => (string)GetValue(UserIconProperty);
            set => SetValue(UserIconProperty, value);
        }

        public static BindableProperty UserIconProperty = BindableProperty.Create(nameof(UserIcon), typeof(string), typeof(SearchBar));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public static BindableProperty PlaceholderTextProperty = BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(SearchBar));

        public ICommand OnClearButtonTapped
        {
            get => (ICommand)GetValue(OnClearButtonTappedProperty);
            set => SetValue(OnClearButtonTappedProperty, value);
        }

        public static BindableProperty OnClearButtonTappedProperty = BindableProperty.Create(nameof(OnClearButtonTapped), typeof(ICommand), typeof(SearchBar));

        public string SearchField
        {
            get => (string)GetValue(SearchFieldProperty);
            set => SetValue(SearchFieldProperty, value);
        }

        public static BindableProperty SearchFieldProperty = BindableProperty.Create(nameof(SearchField), typeof(string), typeof(SearchBar), defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSearchFieldPropertyChanged);

        private static void OnSearchFieldPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var bar = bindable as SearchBar;
            var value = (string)newValue;

            if (string.IsNullOrEmpty(value))
            {
                bar.State = ESearchBarStates.Icon;
            }
            else
            {
                bar.State = ESearchBarStates.Back;
            }
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            SearchCompleteCommand?.Execute(null);
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            SearchCompleteCommand?.Execute(null);
        }
    }
}