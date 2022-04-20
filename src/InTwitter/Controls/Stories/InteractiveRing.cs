using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InTwitter.Controls.Stories
{
    public class InteractiveRing : SKCanvasView
    {
        private static SKPaint _backPaint;
        private static SKPaint _primaryPaint;
        private static int _maxSecond = 30;

        public InteractiveRing()
        {
            this.PaintSurface += OnCanvasViewPaintSurface;

            _backPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5f,
                Color = Color.FromHex("#FCFDFE").ToSKColor(),
            };

            _primaryPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 6f,
                Color = Color.FromHex("#2356C5").ToSKColor(),
            };
        }

        #region --- Public properties ---

        public static readonly BindableProperty SecondCountProperty =
            BindableProperty.Create(
                nameof(SecondCount),
                typeof(DateTimeOffset),
                typeof(SymbolsCounter),
                defaultValue: default(DateTimeOffset),
                defaultBindingMode: BindingMode.OneWay);

        public DateTimeOffset SecondCount
        {
            get => (DateTimeOffset)GetValue(SecondCountProperty);
            set => SetValue(SecondCountProperty, value);
        }

        #endregion

        #region --- Overrides ---

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SecondCount))
            {
                this.InvalidateSurface();
            }
        }

        #endregion

        #region --- Private helpers ---

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface sKSurface = e.Surface;
            SKCanvas sKCanvas = sKSurface.Canvas;
            sKCanvas.Clear();

            var density = e.Info.Width / (float)this.Width;

            sKCanvas.Scale(density);
            float offset = 2f * density;
            SKRect rect = new SKRect(
                offset,
                offset,
                (info.Width / density) - offset,
                (info.Height / density) - offset);

            sKCanvas.DrawOval(rect, _backPaint);

            if (SecondCount.Second != 0 && SecondCount.Second <= _maxSecond)
            {
                float startAngle = -90;
                float sweepAngle = ((float)SecondCount.Second / (float)_maxSecond) * 360f;

                using (SKPath path = new SKPath())
                {
                    path.AddArc(rect, startAngle, sweepAngle);
                    sKCanvas.DrawPath(path, _primaryPaint);
                }
            }
        }

        #endregion
    }
}
