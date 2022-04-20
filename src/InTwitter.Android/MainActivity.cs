using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Views;
using InTwitter.Droid.Renderers;
using LibVLCSharp.Forms.Shared;

namespace InTwitter.Droid
{
    [Activity(Label = "InterTwitter", Icon = "@mipmap/icon", Theme = "@style/MainTheme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private bool _isLieAboutCurrentFocus;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.SetFlags("UseLegacyRenderers");
            LibVLCSharpFormsRenderer.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            var focused = CurrentFocus;
            bool customEntryRendererFocused = focused != null && focused.Parent is CustomFloatingEntryRenderer;

            _isLieAboutCurrentFocus = customEntryRendererFocused;
            var result = base.DispatchTouchEvent(ev);
            _isLieAboutCurrentFocus = false;

            return result;
        }

        public override View CurrentFocus
        {
            get => _isLieAboutCurrentFocus ? null : base.CurrentFocus;
        }
    }
}