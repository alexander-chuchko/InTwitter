using Xamarin.Forms;

namespace InTwitter.Controls.StateContainer.Animation
{
    public class FadeInAnimation : AnimationBase
    {
        public override void Apply(View view)
        {
            if (view != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    view.FadeTo(1);
                });
            }
        }
    }
}