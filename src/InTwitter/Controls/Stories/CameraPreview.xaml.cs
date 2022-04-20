using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InTwitter.Controls.Stories
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPreview : ContentView
    {
        private static DateTimeOffset? _pressTime = null;
        private static DateTimeOffset? _releaseTime = null;

        public CameraPreview()
        {
            InitializeComponent();
        }

        #region --- Public properties ---

        public static readonly BindableProperty MediaElementModeProperty =
            BindableProperty.Create(
                nameof(MediaElementMode),
                typeof(MediaElementState),
                typeof(CameraPreview),
                defaultValue: MediaElementState.Closed,
                propertyChanged: MediaElementModePropertyChanged);

        public MediaElementState MediaElementMode
        {
            get => (MediaElementState)GetValue(MediaElementModeProperty);
            set => SetValue(MediaElementModeProperty, value);
        }

        public static readonly BindableProperty IsRecordingProperty =
            BindableProperty.Create(
                nameof(IsRecording),
                typeof(bool),
                typeof(CameraPreview),
                defaultValue: false,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsRecording
        {
            get => (bool)GetValue(IsRecordingProperty);
            set => SetValue(IsRecordingProperty, value);
        }

        public static readonly BindableProperty TimerVideoProperty =
            BindableProperty.Create(
                nameof(TimerVideo),
                typeof(DateTimeOffset),
                typeof(CameraPreview),
                defaultValue: default(DateTimeOffset),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: TimerVideoPropertyChanged);

        public DateTimeOffset TimerVideo
        {
            get => (DateTimeOffset)GetValue(TimerVideoProperty);
            set => SetValue(TimerVideoProperty, value);
        }

        public static readonly BindableProperty CameraOptionsProperty =
            BindableProperty.Create(
                nameof(CameraOptions),
                typeof(CameraOptions),
                typeof(CameraPreview),
                CameraOptions.Back,
                BindingMode.TwoWay);

        public CameraOptions CameraOptions
        {
            get => (CameraOptions)GetValue(CameraOptionsProperty);
            set => SetValue(CameraOptionsProperty, value);
        }

        public static readonly BindableProperty CaptureModeProperty =
            BindableProperty.Create(
                nameof(CaptureMode),
                typeof(CameraCaptureMode),
                typeof(CameraPreview),
                CameraCaptureMode.Video,
                BindingMode.TwoWay);

        public CameraCaptureMode CaptureMode
        {
            get => (CameraCaptureMode)GetValue(CaptureModeProperty);
            set => SetValue(CaptureModeProperty, value);
        }

        public static readonly BindableProperty PressedButtonCommandProperty =
            BindableProperty.Create(
                nameof(PressedButtonCommand),
                typeof(ICommand),
                typeof(CameraPreview),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: PressedCommandPropertyChanged);

        public ICommand PressedButtonCommand
        {
            get => (ICommand)GetValue(PressedButtonCommandProperty);
            set => SetValue(PressedButtonCommandProperty, value);
        }

        public static readonly BindableProperty OpenShutterProperty =
            BindableProperty.Create(
                nameof(OpenShutter),
                typeof(bool),
                typeof(CameraPreview),
                defaultValue: false,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OpenShutterCommandPropertyChanged);

        public bool OpenShutter
        {
            get => (bool)GetValue(OpenShutterProperty);
            set => SetValue(OpenShutterProperty, value);
        }

        public static readonly BindableProperty TapFlipCameraOrPostCommandProperty =
            BindableProperty.Create(
                nameof(TapFlipCameraOrPostCommand),
                typeof(ICommand),
                typeof(CameraPreview),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: TapFlipCameraOrPostCommandPropertyChanged);

        public ICommand TapFlipCameraOrPostCommand
        {
            get => (ICommand)GetValue(TapFlipCameraOrPostCommandProperty);
            set => SetValue(TapFlipCameraOrPostCommandProperty, value);
        }

        public static readonly BindableProperty PathImageSourceProperty =
            BindableProperty.Create(
                nameof(PathImageSource),
                typeof(string),
                typeof(CameraPreview),
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: PathImagePropertyChanged);

        public string PathImageSource
        {
            get => (string)GetValue(PathImageSourceProperty);
            set => SetValue(PathImageSourceProperty, value);
        }

        public static readonly BindableProperty PathVideoSourceProperty =
            BindableProperty.Create(
                nameof(PathVideoSource),
                typeof(string),
                typeof(CameraPreview),
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: PathVideoPropertyChanged);

        public string PathVideoSource
        {
            get => (string)GetValue(PathVideoSourceProperty);
            set => SetValue(PathVideoSourceProperty, value);
        }

        public static readonly BindableProperty PageStatusProperty =
           BindableProperty.Create(
               nameof(PageStatus),
               typeof(bool),
               typeof(CameraPreview),
               defaultValue: false,
               defaultBindingMode: BindingMode.TwoWay);

        public bool PageStatus
        {
            get => (bool)GetValue(PageStatusProperty);
            set => SetValue(PageStatusProperty, value);
        }

        public static readonly BindableProperty TapCloseCommandProperty =
            BindableProperty.Create(
                nameof(TapCloseCommand),
                typeof(ICommand),
                typeof(CameraPreview),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: TapCloseCommandPropertyChanged);

        public ICommand TapCloseCommand
        {
            get => (ICommand)GetValue(TapCloseCommandProperty);
            set => SetValue(TapCloseCommandProperty, value);
        }

        public static readonly BindableProperty TapMediaStoryCommandProperty =
            BindableProperty.Create(
                nameof(TapMediaStoryCommand),
                typeof(ICommand),
                typeof(CameraPreview),
                defaultValue: default(ICommand),
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: TapMediaStoryCommandPropertyChanged);

        public ICommand TapMediaStoryCommand
        {
            get => (ICommand)GetValue(TapMediaStoryCommandProperty);
            set => SetValue(TapMediaStoryCommandProperty, value);
        }

        public static readonly BindableProperty ImageDataProperty =
            BindableProperty.Create(
                nameof(ImageData),
                typeof(byte[]),
                typeof(CameraPreview),
                defaultValue: default(byte[]),
                defaultBindingMode: BindingMode.TwoWay);

        public byte[] ImageData
        {
            get => (byte[])GetValue(ImageDataProperty);
            set => SetValue(ImageDataProperty, value);
        }

        public static readonly BindableProperty VideoSourceProperty =
            BindableProperty.Create(
                nameof(VideoSource),
                typeof(string),
                typeof(CameraPreview),
                defaultValue: default(string),
                defaultBindingMode: BindingMode.OneWayToSource);

        public string VideoSource
        {
            get => (string)GetValue(VideoSourceProperty);
            set => SetValue(VideoSourceProperty, value);
        }

        #endregion

        #region --- Private helpers ---

        private static void PathVideoPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CameraPreview cameraPreview = bindable as CameraPreview;
            string pathVideoSource = newValue as string;

            if (cameraPreview != null && !string.IsNullOrWhiteSpace(pathVideoSource))
            {
                cameraPreview.previewVideo.Source = (string)newValue;
            }
        }

        private static void PathImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CameraPreview cameraPreview = bindable as CameraPreview;
            string pathImageSource = newValue as string;
            if (cameraPreview != null && !string.IsNullOrWhiteSpace(pathImageSource))
            {
                if (cameraPreview.CaptureMode == CameraCaptureMode.Photo)
                {
                    cameraPreview.previewPicture.Source = (string)newValue;
                }
                else if (cameraPreview.CaptureMode == CameraCaptureMode.Video)
                {
                    cameraPreview.previewVideo.Source = (string)newValue;
                }
            }
        }

        private static void MediaElementModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CameraPreview cameraPreview = bindable as CameraPreview;
            if (cameraPreview != null)
            {
                if (cameraPreview.MediaElementMode == MediaElementState.Stopped)
                {
                    cameraPreview.previewVideo.Stop();
                    cameraPreview.previewVideo.Source = null;
                }
            }
        }

        private static void TimerVideoPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //CameraPreview cameraPreview = bindable as CameraPreview;
            if (bindable is CameraPreview cameraPreview)
            {
                DateTimeOffset dateTimeOffset = (DateTimeOffset)newValue;

                if (dateTimeOffset.Second <= (int)Constants.DURATION_PREVIEW_VIDEO)
                {
                    if (cameraPreview.IsRecording)
                    {
                        cameraPreview.OnExecuteAnimation(1.05);
                    }

                    cameraPreview.interactiveRing.SecondCount = dateTimeOffset;
                    cameraPreview.timerVideo.Text = dateTimeOffset.ToString("mm : ss");
                }
                else if (dateTimeOffset.Second >= (int)Constants.DURATION_PREVIEW_VIDEO)
                {
                    cameraPreview.OnExecuteAnimation(1.0);
                    cameraPreview.PressedButtonCommand.Execute(Constants.LONG_PRESSED_BUTTON);
                }
            }
        }

        private static void TapMediaStoryCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //CameraPreview cameraPreview = bindable as CameraPreview;
            if (bindable is CameraPreview cameraPreview)
            {
                cameraPreview.mediaStory.Command = (ICommand)newValue;
            }
        }

        private static void TapCloseCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //CameraPreview cameraPreview = bindable as CameraPreview;
            if (bindable is CameraPreview cameraPreview)
            {
                cameraPreview.imageClose.Command = (ICommand)newValue;
            }
        }

        private static void TapFlipCameraOrPostCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //CameraPreview cameraPreview = bindable as CameraPreview;
            if (bindable is CameraPreview cameraPreview)
            {
                if (cameraPreview.imageChangeCameraMode.IsVisible == true)
                {
                    cameraPreview.imageChangeCameraMode.Command = (ICommand)newValue;
                }
                else
                {
                    cameraPreview.imageButtonPost.Command = (ICommand)newValue;
                }
            }
        }

        private static void OpenShutterCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //CameraPreview cameraPreview = bindable as CameraPreview;
            if (bindable is CameraPreview cameraPreview)
            {
                if (cameraPreview.OpenShutter == true)
                {
                    if (cameraPreview.cameraPreview.IsAvailable)
                    {
                        cameraPreview.cameraPreview.Shutter();
                    }

                    if (cameraPreview.CaptureMode == CameraCaptureMode.Video && cameraPreview.TimerVideo.Second == 0)
                    {
                        cameraPreview.IsRecording = true;
                    }
                    else if (cameraPreview.CaptureMode == CameraCaptureMode.Video && cameraPreview.TimerVideo.Second != 0)
                    {
                        cameraPreview.IsRecording = false;
                        cameraPreview.OnExecuteAnimation(1.0);
                    }

                    cameraPreview.OpenShutter = false;
                }
            }
        }

        private static void PressedCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //CameraPreview cameraPreview = bindable as CameraPreview;
            if (bindable is CameraPreview cameraPreview)
            {
                cameraPreview.PressedButtonCommand = (ICommand)newValue;
            }
        }

        private void ImageCamera_Tapped(object sender, EventArgs e)
        {
            PageStatus = false;
        }

        private void OnCameraViewMediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            switch (cameraPreview.CaptureMode)
            {
                case CameraCaptureMode.Photo:

                    PageStatus = true;
                    ImageData = e.ImageData;
                    break;

                case CameraCaptureMode.Video:

                    if (previewVideo.Source != null)
                    {
                        previewVideo.Source = null;
                    }

                    VideoSource = e.Video.File;
                    MediaElementMode = MediaElementState.Playing;
                    PageStatus = true;
                    break;
            }
        }

        private void OnExecuteAnimation(double сoefficient = 1.0)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                interactiveRing.ScaleTo(сoefficient, 50);
            });
        }

        private void OnMainButtonTouchStateChanged(object sender, Xamarin.CommunityToolkit.Effects.TouchStateChangedEventArgs e)
        {
            if (e.State == TouchState.Pressed)
            {
                _pressTime = DateTimeOffset.Now;
            }
            else if (e.State == TouchState.Normal)
            {
                _releaseTime = DateTimeOffset.Now;
            }

            if (_pressTime != null && _releaseTime != null)
            {
                TimeSpan differenceTime = (TimeSpan)(_releaseTime - _pressTime);

                if (differenceTime.TotalMilliseconds > 500.0 && differenceTime.TotalMilliseconds <= 30000.0)
                {
                    OnExecuteAnimation(1.0);
                    PressedButtonCommand.Execute(Constants.LONG_PRESSED_BUTTON);
                }

                _pressTime = null;
                _releaseTime = null;
            }
        }

        private void OnCameraViewMediaCaptureFailed(object sender, string e)
        {
        }

        #endregion
    }
}