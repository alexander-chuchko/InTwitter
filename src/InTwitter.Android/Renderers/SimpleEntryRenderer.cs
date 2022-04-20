using Android.Content;
using Android.Graphics.Drawables;
using InTwitter.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(SimpleEntryRenderer))]
namespace InTwitter.Droid.Renderers
{
    public class SimpleEntryRenderer : EntryRenderer
    {
        public SimpleEntryRenderer(Context context) : base(context)
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