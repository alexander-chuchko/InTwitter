using Xamarin.Forms;

namespace InTwitter.Controls.StateContainer.Animation
{
    public class FadeOutAnimation : AnimationBase
    {
        public override void Apply(View view)
        {
            if (view != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    view.FadeTo(0, 1);
                });
            }
        }
    }
}