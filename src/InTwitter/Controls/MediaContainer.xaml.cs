using InTwitter.Models;
using InTwitter.Enums;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public partial class MediaContainer
    {
        public MediaContainer()
        {
            InitializeComponent();
        }

        public ICommand TapCommand
        {
            get => (ICommand)GetValue(TapCommandProperty);
            set => SetValue(TapCommandProperty, value);
        }

        public static BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(MediaContainer));

        public EMediaType Type
        {
            get => (EMediaType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static BindableProperty TypeProperty = BindableProperty.Create(nameof(Type), typeof(EMediaType), typeof(MediaContainer), coerceValue: OnTypePropertyCoerce);

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(MediaContainer));

        public bool AllowVideo
        {
            get => (bool)GetValue(AllowVideoProperty);
            set => SetValue(AllowVideoProperty, value);
        }

        public static BindableProperty AllowVideoProperty = BindableProperty.Create(nameof(AllowVideo), typeof(bool), typeof(MediaContainer));

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public static BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(MediaContainer), true);

        private static object OnTypePropertyCoerce(BindableObject bindable, object value)
        {
            var container = bindable as MediaContainer;
            var newValue = (EMediaType)value;

            switch (newValue)
            {
                case EMediaType.Gif:
                    container.labelImage.Source = ImageSource.FromFile("pic_gif.png");
                    container.labelImageFrame.HorizontalOptions = LayoutOptions.End;
                    container.labelImageFrame.VerticalOptions = LayoutOptions.End;
                    break;
                case EMediaType.Video:
                    if (container.AllowVideo == true)
                    {
                        var video = new VideoContainer();
                        video.Source = (container.BindingContext as MediaSourceViewModel).MediaSource;
                        container.gridContainer.Children.Add(video, 0, 0);
                    }
                    else
                    {
                        container.labelImage.Source = ImageSource.FromUri(new Uri("https://cdn.futuredealer.com/app/sites/12/2018/08/video-placeholder.png"));
                        container.labelImageFrame.HorizontalOptions = LayoutOptions.Center;
                        container.labelImageFrame.VerticalOptions = LayoutOptions.Center;
                        container.labelImage.Source = ImageSource.FromFile("ic_play.png");
                        container.labelImageFrame.Padding = new Thickness(8);
                        container.labelImageFrame.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        container.labelImageFrame.SizeChanged += container.OnLabelImageFrameSizeChanged;
                    }

                    break;
            }

            return value;
        }

        private void OnLabelImageFrameSizeChanged(object sender, EventArgs e)
        {
            labelImageFrame.CornerRadius = (float)((labelImageFrame.Height + labelImageFrame.Width) / 4.0);
            (sender as Frame).SizeChanged -= OnLabelImageFrameSizeChanged;
        }

        private void CachedImage_Finish(object sender, FFImageLoading.Forms.CachedImageEvents.FinishEventArgs e)
        {
            IsLoading = false;

            (sender as FFImageLoading.Forms.CachedImage).Finish -= CachedImage_Finish;
        }
    }
}