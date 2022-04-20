using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Views.Templates
{
    public partial class NotificationTemplate
    {
        public NotificationTemplate()
        {
            InitializeComponent();
        }

        public static BindableProperty TweetTappedProperty =
            BindableProperty.Create(
                                    nameof(TweetTapped),
                                    typeof(ICommand),
                                    typeof(NotificationTemplate));

        public ICommand TweetTapped
        {
            get => (ICommand)GetValue(TweetTappedProperty);
            set => SetValue(TweetTappedProperty, value);
        }

        public static BindableProperty UserTappedProperty =
            BindableProperty.Create(
                                    nameof(UserTapped),
                                    typeof(ICommand),
                                    typeof(NotificationTemplate));

        public ICommand UserTapped
        {
            get => (ICommand)GetValue(UserTappedProperty);
            set => SetValue(UserTappedProperty, value);
        }

        public static BindableProperty ImageVideoTappedProperty =
            BindableProperty.Create(
                                    nameof(ImageVideoTapped),
                                    typeof(ICommand),
                                    typeof(NotificationTemplate));

        public ICommand ImageVideoTapped
        {
            get => (ICommand)GetValue(ImageVideoTappedProperty);
            set => SetValue(ImageVideoTappedProperty, value);
        }
    }
}