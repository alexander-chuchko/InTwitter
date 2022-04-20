using System;
using System.Collections.Generic;
using InTwitter.Controls;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;

namespace AgentLocator.iOS
{
    public class UITouchesGestureRecognizer : UIGestureRecognizer
    {
        #region Private Members

        /// <summary>
        /// The element.
        /// </summary>
        private ClickableContentView _element;

        /// <summary>
        /// The native view.
        /// </summary>
        private UIView _nativeView;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UITouchesGestureRecognizer"/> class.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="nativeView">Native view.</param>
        public UITouchesGestureRecognizer(ClickableContentView element, UIView nativeView)
        {
            if (null == element)
            {
                throw new ArgumentNullException("element");
            }

            if (null == nativeView)
            {
                throw new ArgumentNullException("nativeView");
            }

            _element = element;
            _nativeView = nativeView;
        }

        /// <summary>
        /// Gets the touch points.
        /// </summary>
        /// <returns>The touch points.</returns>
        /// <param name="touches">Touches.</param>
        private IEnumerable<Point> GetTouchPoints(
            Foundation.NSSet touches)
        {
            var points = new List<Point>((int)touches.Count);
            foreach (UITouch touch in touches)
            {
                CGPoint touchPoint = touch.LocationInView(_nativeView);
                points.Add(new Point((double)touchPoint.X, (double)touchPoint.Y));
            }

            return points;
        }

        /// <summary>
        /// Toucheses the began.
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (this._element.TouchesBegan(GetTouchPoints(touches)))
                this.State = UIGestureRecognizerState.Began;
            else
                this.State = UIGestureRecognizerState.Cancelled;
        }

        /// <summary>
        /// Toucheses the moved.
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (this._element.TouchesMoved(GetTouchPoints(touches)))
                this.State = UIGestureRecognizerState.Changed;
        }

        /// <summary>
        /// Toucheses the ended.
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (this._element.TouchesEnded(GetTouchPoints(touches)))
                this.State = UIGestureRecognizerState.Ended;
        }

        /// <summary>
        /// Toucheses the cancelled.
        /// </summary>
        /// <param name="touches">Touches.</param>
        /// <param name="evt">Evt.</param>
        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            this._element.TouchesCancelled(GetTouchPoints(touches));
            this.State = UIGestureRecognizerState.Cancelled;
        }
    }
}