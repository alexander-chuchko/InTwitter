using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public partial class FeedView
    {
        private double _oldScrollPosition;

        public FeedView()
        {
            this.InitializeComponent();

            this.Scrolled += this.FeedView_Scrolled;
        }

        #region ---Public Properties---

        public StackLayout Header
        {
            get => (StackLayout)this.GetValue(HeaderProperty);
            set => this.SetValue(HeaderProperty, value);
        }

        public static BindableProperty HeaderProperty = BindableProperty.Create(
            nameof(Header),
            typeof(StackLayout),
            typeof(FeedView),
            coerceValue: OnHeaderPropertyCoerce);

        public StackLayout Footer
        {
            get => (StackLayout)this.GetValue(FooterProperty);
            set => this.SetValue(FooterProperty, value);
        }

        public static BindableProperty FooterProperty = BindableProperty.Create(
            nameof(Footer),
            typeof(StackLayout),
            typeof(FeedView),
            coerceValue: OnFooterPropertyCoerce);

        public IEnumerable ItemSource
        {
            get => (IEnumerable)this.GetValue(ItemSourceProperty);
            set => this.SetValue(ItemSourceProperty, value);
        }

        public static BindableProperty ItemSourceProperty = BindableProperty.Create(
            nameof(ItemSource),
            typeof(IEnumerable),
            typeof(FeedView),
            coerceValue: OnItemSourcePropertyChanged);

        public ICommand OnFinishedScrollCommand
        {
            get => (ICommand)GetValue(OnFinishedScrollCommandProperty);
            set => SetValue(OnFinishedScrollCommandProperty, value);
        }

        public static BindableProperty OnFinishedScrollCommandProperty = BindableProperty.Create(
            nameof(OnFinishedScrollCommand),
            typeof(ICommand),
            typeof(FeedView));

        public DataTemplate DataTemplate
        {
            get => (DataTemplate)GetValue(DataTemplateProperty);
            set => SetValue(DataTemplateProperty, value);
        }

        public static BindableProperty DataTemplateProperty = BindableProperty.Create(
            nameof(DataTemplate),
            typeof(DataTemplate),
            typeof(FeedView),
            coerceValue: OnDataTemplatePropertyChanged);

        public double ScrollPosition
        {
            get => (double)GetValue(ScrollPositionProperty);
            set => SetValue(ScrollPositionProperty, value);
        }

        public static BindableProperty ScrollPositionProperty = BindableProperty.Create(
            nameof(ScrollPosition),
            typeof(double),
            typeof(FeedView),
            defaultBindingMode: BindingMode.OneWayToSource);

        public bool TappedFeed
        {
            get => (bool)GetValue(TappedFeedProperty);
            set => SetValue(TappedFeedProperty, value);
        }

        public static BindableProperty TappedFeedProperty = BindableProperty.Create(
            nameof(TappedFeed),
            typeof(bool),
            typeof(FeedView),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        #endregion

        #region ---Event Handlers---

        private static object OnHeaderPropertyCoerce(BindableObject bindable, object value)
        {
            var feedView = (FeedView)bindable;
            var newStack = value as StackLayout;

            (feedView.Content as StackLayout).Children.Insert(0, newStack);

            return value;
        }

        private static object OnFooterPropertyCoerce(BindableObject bindable, object value)
        {
            var feedView = (FeedView)bindable;
            var newStack = value as StackLayout;

            (feedView.Content as StackLayout).Children.Insert(2, newStack);

            return value;
        }

        private static object OnItemSourcePropertyChanged(BindableObject bindable, object newValue)
        {
            var feedView = (FeedView)bindable;

            BindableLayout.SetItemsSource(feedView.contentLayout, newValue as IEnumerable);

            return newValue;
        }

        private static object OnDataTemplatePropertyChanged(BindableObject bindable, object newValue)
        {
            var feedView = (FeedView)bindable;

            BindableLayout.SetItemTemplate(feedView.contentLayout, newValue as DataTemplate);

            return newValue;
        }

        private async void FeedView_Scrolled(object sender, ScrolledEventArgs e)
        {
            this.ScrollPosition = e.ScrollY;

            if (this.contentLayout.Height - e.ScrollY - this.Height < (this.Height / 3.0))
            {
                if (this.ScrollPosition - _oldScrollPosition > 0)
                {
                    this.OnFinishedScrollCommand?.Execute(null);
                }
            }

            _oldScrollPosition = this.ScrollPosition;
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            TappedFeed = true;
        }

        #endregion
    }
}