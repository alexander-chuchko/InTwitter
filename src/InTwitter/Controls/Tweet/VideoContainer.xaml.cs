using Xamarin.Forms;
using LibVLCSharp.Shared;
using InTwitter.Helpers;

namespace InTwitter.Controls
{
    public partial class VideoContainer
    {
        public VideoContainer()
        {
            InitializeComponent();
        }

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(string), typeof(VideoContainer), propertyChanged: OnSourcePropertyChanged);

        private static async void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var container = bindable as VideoContainer;
            var source = (string)newValue;

            if (!string.IsNullOrEmpty(source))
            {
                var media = new Media(MediaCenter.Library, source, source.StartsWith('/') ? FromType.FromPath : FromType.FromLocation);
                container.videoPlayer.MediaPlayer = MediaCenter.GetPlayer(media);
                container.videoPlayer.MediaPlayer.Play();

                await System.Threading.Tasks.Task.Delay(1500);

                container.videoPlayer.MediaPlayer.Pause();
            }
        }
    }
}