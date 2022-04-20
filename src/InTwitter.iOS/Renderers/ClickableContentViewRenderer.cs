using System;
using AgentLocator.iOS;
using InTwitter.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ClickableContentView), typeof(InTwitter.iOS.Renderers.Controls.ClickableContentViewRenderer))]

namespace InTwitter.iOS.Renderers.Controls
{
    [Preserve(AllMembers = true)]
#pragma warning disable SA1600 // Elements should be documented
    public class ClickableContentViewRenderer : VisualElementRenderer<ClickableContentView>
#pragma warning restore SA1600 // Elements should be documented
    {
        private UITouchesGestureRecognizer _gestureRecognizer;

        public static new void Init()
        {
            var temp = DateTime.Now;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ClickableContentView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                if (_gestureRecognizer != null)
                {
                    RemoveGestureRecognizer(_gestureRecognizer);

                    _gestureRecognizer = null;
                }

                e.OldElement.OnInvalidate -= HandleInvalidate;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnInvalidate += HandleInvalidate;

                if ((_gestureRecognizer == null) && (NativeView != null))
                {
                    _gestureRecognizer = new UITouchesGestureRecognizer(e.NewElement, NativeView);

                    NativeView.AddGestureRecognizer(_gestureRecognizer);
                }

                e.NewElement.Invalidate();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ClickableContentView.IsClippedToBoundsProperty.PropertyName)
            {
                Layer.MasksToBounds = Element.IsClippedToBounds;
            }
            else if (e.PropertyName == ClickableContentView.BackgroundColorProperty.PropertyName)
            {
                this.Element.Invalidate();
            }
            else if (e.PropertyName == ClickableContentView.IsVisibleProperty.PropertyName)
            {
                Element.Invalidate();
            }
        }

        #region -- Touch Handlers --

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
        }

        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
        }

        #endregion

        #region -- Private Members --

        private void HandleInvalidate(object sender, System.EventArgs args)
        {
            SetNeedsDisplay();
        }

        #endregion
    }
}