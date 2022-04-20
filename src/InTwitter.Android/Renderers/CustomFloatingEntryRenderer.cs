using Android.Content;
using Android.Graphics.Drawables;
using InTwitter.Controls.FloatingEntry;
using InTwitter.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FloatEntry), typeof(CustomFloatingEntryRenderer))]
namespace InTwitter.Droid.Renderers
{
    class CustomFloatingEntryRenderer : EntryRenderer
    {
        public CustomFloatingEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
           
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);

                if (!Element.IsPassword)
                {
                    Control.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
                }
            }
        }
    }
}