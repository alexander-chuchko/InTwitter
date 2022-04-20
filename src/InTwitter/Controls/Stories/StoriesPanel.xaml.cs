using InTwitter.Models.Icon;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls.Stories
{
    public partial class StoriesPanel : ContentView
    {
        public StoriesPanel()
        {
            InitializeComponent();
        }

        #region --- Public properties ---

        public static readonly BindableProperty ScrollStateProperty =
            BindableProperty.Create(
                nameof(ScrollState),
                typeof(double),
                typeof(StoriesPanel),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: ScrollStatePropertyChanged);

        public double ScrollState
        {
            get => (double)GetValue(ScrollStateProperty);
            set => SetValue(ScrollStateProperty, value);
        }

        public static readonly BindableProperty UserStoriesListProperty =
            BindableProperty.Create(
                nameof(UserStoriesList),
                typeof(ObservableCollection<UserStoryViewModel>),
                typeof(StoriesPanel),
                defaultValue: default(ObservableCollection<UserStoryViewModel>),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnUserStoriesListPropertyChanged);

        public ObservableCollection<UserStoryViewModel> UserStoriesList
        {
            get => (ObservableCollection<UserStoryViewModel>)GetValue(UserStoriesListProperty);
            set => SetValue(UserStoriesListProperty, value);
        }

        public static readonly BindableProperty SelectedUserStoryProperty =
            BindableProperty.Create(
                nameof(SelectedUserStory),
                typeof(UserStory),
                typeof(StoriesPanel),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay);

        public UserStory SelectedUserStory
        {
            get => (UserStory)GetValue(SelectedUserStoryProperty);
            set => SetValue(SelectedUserStoryProperty, value);
        }

        public static readonly BindableProperty TapUserStoryCommandProperty =
            BindableProperty.Create(
                nameof(TapUserStoryCommand),
                typeof(ICommand),
                typeof(StoriesPanel),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand TapUserStoryCommand
        {
            get => (ICommand)GetValue(TapUserStoryCommandProperty);
            set => SetValue(TapUserStoryCommandProperty, value);
        }

        public static readonly BindableProperty PressedButtonCommandProperty =
            BindableProperty.Create(
                nameof(PressedButtonCommand),
                typeof(ICommand),
                typeof(StoriesPanel),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: PressedCommandPropertyChanged);

        public ICommand PressedButtonCommand
        {
            get => (ICommand)GetValue(PressedButtonCommandProperty);
            set => SetValue(PressedButtonCommandProperty, value);
        }

        #endregion

        #region --- Private helpers ---

        private static void PressedCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StoriesPanel storiesPanel)
            {
                storiesPanel.PressedButtonCommand = (ICommand)newValue;
            }
        }

        private static void OnUserStoriesListPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StoriesPanel storiesPanel)
            {
                BindableLayout.SetItemsSource(storiesPanel.collectionStoriesList, (ObservableCollection<UserStoryViewModel>)newValue);
            }
        }

        private static void ScrollStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StoriesPanel storiesPanel)
            {
                if ((double)newValue >= 0)
                {
                    double offset = storiesPanel.TranslationY + (double)oldValue - (double)newValue;

                    offset = Math.Min(offset, 0);
                    var result = -52 - storiesPanel.Height;
                    offset = Math.Max(offset, result);
                    storiesPanel.TranslationY = offset;
                }
            }
        }

        #endregion
    }
}