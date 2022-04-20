using InTwitter.Enums;
using InTwitter.Models.MediaSource;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InTwitter.Controls.Stories
{
    public partial class ViewingStories : ContentView
    {
        private static double _widhtProgressBar = 0.0;

        private static CancellationToken _cancellationToken = CancellationToken.None;
        public ViewingStories()
        {
            InitializeComponent();
        }

        #region ---   Public properties   ---

        public static readonly BindableProperty TimeSincePublicationProperty =
            BindableProperty.Create(
                nameof(TimeSincePublication),
                typeof(string),
                typeof(ViewingStories),
                defaultValue: default(string),
                defaultBindingMode: BindingMode.TwoWay);

        public string TimeSincePublication
        {
            get => (string)GetValue(TimeSincePublicationProperty);
            set => SetValue(TimeSincePublicationProperty, value);
        }

        public static readonly BindableProperty UserPictureSourceProperty =
            BindableProperty.Create(
                nameof(UserPictureSource),
                typeof(string),
                typeof(ViewingStories),
                defaultValue: default(string),
                defaultBindingMode: BindingMode.OneWay);
        public string UserPictureSource
        {
            get => (string)GetValue(UserPictureSourceProperty);
            set => SetValue(UserPictureSourceProperty, value);
        }

        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(
                nameof(Name),
                typeof(string),
                typeof(ViewingStories),
                defaultValue: default(string),
                defaultBindingMode: BindingMode.OneWay);
        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public static readonly BindableProperty StatusMediaTypeProperty =
            BindableProperty.Create(
                nameof(StatusMediaType),
                typeof(EMediaType),
                typeof(ViewingStories),
                EMediaType.Image,
                BindingMode.TwoWay);

        public EMediaType StatusMediaType
        {
            get => (EMediaType)GetValue(StatusMediaTypeProperty);
            set => SetValue(StatusMediaTypeProperty, value);
        }

        public static readonly BindableProperty TapCommandProperty =
            BindableProperty.Create(
                nameof(TapCommand),
                typeof(ICommand),
                typeof(CameraPreview),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay);

        public ICommand TapCommand
        {
            get => (ICommand)GetValue(TapCommandProperty);
            set => SetValue(TapCommandProperty, value);
        }

        public static readonly BindableProperty NumberIndicatorsProperty =
            BindableProperty.Create(
                nameof(NumberIndicators),
                typeof(List<ProgressBar>),
                typeof(ViewingStories),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: NumberIndicatorsPropertyChanged);

        public List<ProgressBar> NumberIndicators
        {
            get => (List<ProgressBar>)GetValue(NumberIndicatorsProperty);
            set => SetValue(NumberIndicatorsProperty, value);
        }

        public static readonly BindableProperty CancelTokenProperty =
            BindableProperty.Create(
                nameof(CancelTokenProperty),
                typeof(CancellationTokenSource),
                typeof(ViewingStories),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay);

        public CancellationTokenSource CancelToken
        {
            get => (CancellationTokenSource)GetValue(CancelTokenProperty);
            set => SetValue(CancelTokenProperty, value);
        }

        public static readonly BindableProperty SelectedItemMediaProperty =
            BindableProperty.Create(
                nameof(SelectedItemMedia),
                typeof(MediaStorySource),
                typeof(ViewingStories),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay);
        public MediaStorySource SelectedItemMedia
        {
            get => (MediaStorySource)GetValue(SelectedItemMediaProperty);
            set => SetValue(SelectedItemMediaProperty, value);
        }

        public static readonly BindableProperty ListMediaStoriesProperty =
            BindableProperty.Create(
                nameof(ListMediaStories),
                typeof(List<MediaStorySource>),
                typeof(ViewingStories),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: ListMediaStoriesPropertyChanged);

        public List<MediaStorySource> ListMediaStories//To do
        {
            get => (List<MediaStorySource>)GetValue(ListMediaStoriesProperty);
            set => SetValue(ListMediaStoriesProperty, value);
        }

        public static readonly BindableProperty PageStatusProperty =
           BindableProperty.Create(
               nameof(PageStatus),
               typeof(bool),
               typeof(ViewingStories),
               defaultValue: true,
               defaultBindingMode: BindingMode.TwoWay);

        public bool PageStatus
        {
            get => (bool)GetValue(PageStatusProperty);
            set => SetValue(PageStatusProperty, value);
        }

        public static readonly BindableProperty StateProgressBarProperty =
            BindableProperty.Create(
                nameof(StateProgressBar),
                typeof((double, double)),
                typeof(ViewingStories),
                defaultValue: default((double, double)),
                defaultBindingMode: BindingMode.TwoWay);

        public (double, double) StateProgressBar
        {
            get => ((double, double))GetValue(StateProgressBarProperty);
            set => SetValue(StateProgressBarProperty, value);
        }

        #endregion

        #region ---   Private helpers   ---

        private static async void ListMediaStoriesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ViewingStories viewingStories = bindable as ViewingStories;

            if (newValue != null)
            {
                viewingStories.stackLayoutStoriesProgress.Children.Clear();
                var displayInfo = DeviceDisplay.MainDisplayInfo;
                _widhtProgressBar = ((displayInfo.Width / displayInfo.Density) -
                    (viewingStories.parentStackLayout.Padding.Left + viewingStories.parentStackLayout.Padding.Right) -
                    (viewingStories.stackLayoutStoriesProgress.Spacing * (viewingStories.ListMediaStories.Count - 1))) /
                    viewingStories.ListMediaStories.Count;

                viewingStories.CreateProgressBar(viewingStories);

                if (_cancellationToken.IsCancellationRequested == true)
                {
                    _cancellationToken = CancellationToken.None;
                }

                if (_cancellationToken == CancellationToken.None)
                {
                    _cancellationToken = viewingStories.CancelToken.Token;
                }

                if (viewingStories.previewVideo.Source != null)
                {
                    viewingStories.previewVideo.Source = null;
                }

                await viewingStories.ToStartPrevewing(_cancellationToken);
            }
        }

        private static void NumberIndicatorsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ViewingStories viewingStories)
            {
                foreach (var indicator in viewingStories.NumberIndicators)
                {
                    viewingStories.stackLayoutStoriesProgress.Children.Add((ProgressBar)indicator);
                }
            }
        }

        private void CreateProgressBar(ViewingStories viewingStories)
        {
            for (int i = 0; i < viewingStories.ListMediaStories.Count; i++)
            {
                viewingStories.stackLayoutStoriesProgress.Children.Add(
                new ProgressBar
                {
                    BackgroundColor = Color.FromHex("#9D9FA3"),
                    WidthRequest = ViewingStories._widhtProgressBar,
                    ProgressColor = Color.FromHex("#2356C5"),
                    HeightRequest = 1.5,
                });
            }
        }

        private async Task ToStartPrevewing(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (previewVideo.Source == null)
            {
                int counter = 0;
                int index = ListMediaStories.IndexOf(SelectedItemMedia);

                foreach (var progressBar in stackLayoutStoriesProgress.Children)
                {
                    var indicatorProgressBar = (Xamarin.Forms.ProgressBar)progressBar;
                    double stepProgressBar = Constants.PROGRESS_BAR_MIN_VALUE;
                    SelectedItemMedia = ListMediaStories[counter];

                    if (counter >= index)
                    {
                        TimeSpan difference = DateTimeOffset.Now - ListMediaStories[counter].PublicationTime;
                        TimeSincePublication = " " + difference.Minutes + " " + "min.";
                        InitializationPropertySource(counter);

                        if (PageStatus && StateProgressBar.Item1 != Constants.PROGRESS_BAR_MIN_VALUE)
                        {
                            stepProgressBar = StateProgressBar.Item1;
                        }

                        double startTime = GetFirstIndex();
                        double viewingDuration = GetSecondIndex(counter);

                        for (double i = startTime; i < (viewingDuration * Constants.PROGRESS_BAR_SPEED); i++)
                        {
                            if (counter < index)
                            {
                                indicatorProgressBar.Progress = Constants.PROGRESS_BAR_MAX_VALUE;
                                break;
                            }

                            stepProgressBar += 1.0 / (viewingDuration * Constants.PROGRESS_BAR_SPEED);

                            indicatorProgressBar.Progress = stepProgressBar;

                            if (cancellationToken.IsCancellationRequested)
                            {
                                PauseOrStopPlayVideo(counter, Constants.STOP_PLAY);

                                RememberStateProgressBar(viewingDuration, i, stepProgressBar);

                                return;
                            }

                            ResumeStateProgressBar();

                            await Task.Delay(100);
                        }

                        PauseOrStopPlayVideo(counter, Constants.PAUSE_PLAY);

                        counter++;
                    }
                    else
                    {
                        indicatorProgressBar.Progress = Constants.PROGRESS_BAR_MAX_VALUE;
                        counter++;
                        continue;
                    }
                }

                await Task.Delay(50);

                TapCommand.Execute(Constants.CLOSING_PAGE);
            }
        }

        private double GetFirstIndex()
        {
            double startTime = PageStatus
                && StateProgressBar.Item2
                != default(double)
                ? StateProgressBar.Item2
                : default(double);

            return startTime;
        }

        private double GetSecondIndex(int counter)
        {
            var viewingDuration = ListMediaStories[counter].MediaType
                == EMediaType.Image
                ? Constants.DURATION_PREVIEW_PICTURE
                : ListMediaStories[counter].DurationVideo;

            return viewingDuration;
        }

        private void InitializationPropertySource(int counter)
        {
            switch (ListMediaStories[counter].MediaType)
            {
                case EMediaType.Image:

                    StatusMediaType = EMediaType.Image;
                    previewPicture.Source = ListMediaStories[counter].MediaSource;
                    break;

                case EMediaType.Video:

                    StatusMediaType = EMediaType.Video;
                    previewVideo.Source = ListMediaStories[counter].MediaSource;
                    break;
            }
        }

        private void ResumeStateProgressBar()
        {
            if (StateProgressBar.Item2 != 0.0 && StateProgressBar.Item1 != 0.0)
            {
                StateProgressBar = (default(double), default(double));
            }
        }

        private void RememberStateProgressBar(double viewingDuration, double i, double stepProgressBar)
        {
            if (PageStatus && i < (viewingDuration * Constants.PROGRESS_BAR_SPEED))
            {
                var stateProgressBar = (stepProgressBar, i);
                StateProgressBar = stateProgressBar;
            }
        }

        private void PauseOrStopPlayVideo(int counter, string parametr)
        {
            if (ListMediaStories[counter].MediaType == EMediaType.Video && !string.IsNullOrEmpty(parametr))
            {
                if (parametr == Constants.PAUSE_PLAY)
                {
                    previewVideo.Pause();
                }
                else if (parametr == Constants.STOP_PLAY)
                {
                    previewVideo.Stop();
                }

                if (previewVideo.Source != null)
                {
                    previewVideo.Source = null;
                }
            }
        }

        #endregion
    }
}