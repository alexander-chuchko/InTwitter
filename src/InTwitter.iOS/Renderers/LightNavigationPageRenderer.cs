using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using InTwitter.Views;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(InTwitter.iOS.Renderers.LightNavigationPageRenderer))]
[assembly: ExportRenderer(typeof(TweetMediaPage), typeof(InTwitter.iOS.Renderers.TweetMediaPageRenderer))]
namespace InTwitter.iOS.Renderers
{
    public class LightNavigationPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            try
            {
               this.OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }
            catch (Exception ex)
            {
            }

        }
    }

    public class TweetMediaPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            (Element as Page).Appearing += TweetMediaPageRenderer_Appearing;
            (Element as Page).Disappearing += TweetMediaPageRenderer_Disappearing;
        }

        private void TweetMediaPageRenderer_Disappearing(object sender, EventArgs e)
        {
            Page page = this.Element as Page;

            while (page.GetType() != typeof(NavigationPage))
            {
                page = page.Parent as Page;
            }

            (page as NavigationPage).BarTextColor = Color.Black;
        }

        private void TweetMediaPageRenderer_Appearing(object sender, EventArgs e)
        {
            Page page = this.Element as Page;

            while (page.GetType() != typeof(NavigationPage))
            {
                page = page.Parent as Page;
            }

            (page as NavigationPage).BarTextColor = Color.White;
            
        }

    }
}