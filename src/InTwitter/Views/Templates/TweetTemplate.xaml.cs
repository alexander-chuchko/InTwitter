using Prism.Commands;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using InTwitter.Enums;

namespace InTwitter.Views.Templates
{
    public partial class TweetTemplate
    {
        public TweetTemplate()
        {
            InitializeComponent();
        }

        public ETweetTemplateDestiny TweetDestiny
        {
            get => (ETweetTemplateDestiny)GetValue(TweetDestinyProperty);
            set => SetValue(TweetDestinyProperty, value);
        }

        public static BindableProperty TweetDestinyProperty = BindableProperty.Create(nameof(TweetDestiny), typeof(ETweetTemplateDestiny), typeof(TweetTemplate));

        public ICommand UserTappedCommand
        {
            get => (ICommand)GetValue(UserTappedCommandProperty);
            set => SetValue(UserTappedCommandProperty, value);
        }

        public static BindableProperty UserTappedCommandProperty = BindableProperty.Create(nameof(UserTappedCommand), typeof(ICommand), typeof(TweetTemplate));

        public ICommand TweetTappedCommand
        {
            get => (ICommand)GetValue(TweetTappedCommandProperty);
            set => SetValue(TweetTappedCommandProperty, value);
        }

        public static BindableProperty TweetTappedCommandProperty = BindableProperty.Create(nameof(TweetTappedCommand), typeof(ICommand), typeof(TweetTemplate));

        public ICommand MediaTappedCommand
        {
            get => (ICommand)GetValue(MediaTappedCommandProperty);
            set => SetValue(MediaTappedCommandProperty, value);
        }

        public static BindableProperty MediaTappedCommandProperty = BindableProperty.Create(nameof(MediaTappedCommand), typeof(ICommand), typeof(TweetTemplate));

        public ICommand HashtagTappedCommand
        {
            get => (ICommand)GetValue(HashtagTappedCommandProperty);
            set => SetValue(HashtagTappedCommandProperty, value);
        }

        public static BindableProperty HashtagTappedCommandProperty = BindableProperty.Create(nameof(HashtagTappedCommand), typeof(ICommand), typeof(TweetTemplate));

        public ICommand MoreTextTappedCommand
        {
            get => (ICommand)GetValue(MoreTextTappedCommandProperty);
            set => SetValue(MoreTextTappedCommandProperty, value);
        }

        public static BindableProperty MoreTextTappedCommandProperty = BindableProperty.Create(nameof(MoreTextTappedCommand), typeof(ICommand), typeof(TweetTemplate));

        public string AccentedText
        {
            get => (string)GetValue(AccentedTextProperty);
            set => SetValue(AccentedTextProperty, value);
        }

        public static BindableProperty AccentedTextProperty = BindableProperty.Create(nameof(AccentedText), typeof(string), typeof(TweetTemplate));

        public ICommand LikeTappedCommand
        {
            get => (ICommand)GetValue(LikeTappedCommandProperty);
            set => SetValue(LikeTappedCommandProperty, value);
        }

        public static BindableProperty LikeTappedCommandProperty = BindableProperty.Create(nameof(LikeTappedCommand), typeof(ICommand), typeof(TweetTemplate));

        public ICommand MarkTappedCommand
        {
            get => (ICommand)GetValue(MarkTappedCommandProperty);
            set => SetValue(MarkTappedCommandProperty, value);
        }

        public static BindableProperty MarkTappedCommandProperty = BindableProperty.Create(nameof(MarkTappedCommand), typeof(ICommand), typeof(TweetTemplate));
    }
}