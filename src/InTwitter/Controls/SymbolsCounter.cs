using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public class SymbolsCounter : SKCanvasView
    {
        private static SKPaint _backPaint;
        private static SKPaint _primaryPaint;
        private static SKPaint _errorPaint;
        private static SKPaint _digitsFillPaint;

        private static SKRect _circle;

        public SymbolsCounter()
        {
            _backPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
            };

            _primaryPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
            };

            _errorPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
            };

            _digitsFillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                TextSize = 40,
            };

            this.PaintSurface += OnCustomCanvas_PaintSurface;
        }

        #region -- Public properties --

        public static readonly BindableProperty SymbolsCountProperty =
            BindableProperty.Create(
                                    nameof(SymbolsCount),
                                    typeof(int),
                                    typeof(SymbolsCounter),
                                    defaultValue: default(int),
                                    propertyChanged: SymbolCountPropertyChanged);

        public int SymbolsCount
        {
            get => (int)GetValue(SymbolsCountProperty);
            set => SetValue(SymbolsCountProperty, value);
        }

        public static readonly BindableProperty MaxCountProperty =
            BindableProperty.Create(
                                    nameof(MaxCount),
                                    typeof(int),
                                    typeof(SymbolsCounter),
                                    defaultValue: default(int));

        public int MaxCount
        {
            get => (int)GetValue(MaxCountProperty);
            set => SetValue(MaxCountProperty, value);
        }

        public static readonly BindableProperty BackColorProperty =
            BindableProperty.Create(
                            nameof(BackColor),
                            typeof(Color),
                            typeof(SymbolsCounter),
                            defaultValue: default(Color),
                            propertyChanged: BackColorPropertyChanged);

        public Color BackColor
        {
            get => (Color)GetValue(SymbolsCountProperty);
            set => SetValue(SymbolsCountProperty, value);
        }

        public static readonly BindableProperty PrimaryColorProperty =
            BindableProperty.Create(
                            nameof(PrimaryColor),
                            typeof(Color),
                            typeof(SymbolsCounter),
                            defaultValue: default(Color),
                            propertyChanged: PrimaryColorPropertyChanged);

        public Color PrimaryColor
        {
            get => (Color)GetValue(SymbolsCountProperty);
            set => SetValue(SymbolsCountProperty, value);
        }

        public static readonly BindableProperty ErrorColorProperty =
            BindableProperty.Create(
                            nameof(ErrorColor),
                            typeof(Color),
                            typeof(SymbolsCounter),
                            defaultValue: default(Color),
                            propertyChanged: ErrorColorPropertyChanged);

        public Color ErrorColor
        {
            get => (Color)GetValue(SymbolsCountProperty);
            set => SetValue(SymbolsCountProperty, value);
        }

        #endregion

        #region --Overrides--

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SymbolsCount) || propertyName == nameof(MaxCount))
            {
                this.InvalidateSurface();
            }
        }

        #endregion

        #region --Private helpers--

        private static void SymbolCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void BackColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SymbolsCounter canvas = bindable as SymbolsCounter;

            if (canvas != null)
            {
                _backPaint.Color = ((Color)newValue).ToSKColor();
            }
        }

        private static void PrimaryColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SymbolsCounter canvas = bindable as SymbolsCounter;

            if (canvas != null)
            {
                _primaryPaint.Color = ((Color)newValue).ToSKColor();
            }
        }

        private static void ErrorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SymbolsCounter canvas = bindable as SymbolsCounter;

            if (canvas != null)
            {
                _errorPaint.Color = ((Color)newValue).ToSKColor();
                _digitsFillPaint.Color = ((Color)newValue).ToSKColor();
            }
        }

        private void OnCustomCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            int width = e.Info.Width;
            int height = e.Info.Height;

            float radius = width < height ? (width / 2) * 0.83f : (height / 2) * 0.83f;

            canvas.Clear(SKColors.Transparent);

            canvas.DrawCircle(width / 2, height / 2, radius, _backPaint);

            if (MaxCount != 0 && SymbolsCount != 0)
            {
                if (SymbolsCount <= MaxCount)
                {
                    float angle = ((float)SymbolsCount / (float)MaxCount) * 360f;

                    _circle = new SKRect(width * 0.08f, height * 0.08f, width * 0.92f, height * 0.92f);
                    using (SKPath path = new SKPath())
                    {
                        path.AddArc(_circle, -90, angle);
                        canvas.DrawPath(path, _primaryPaint);
                    }
                }
                else
                {
                    canvas.DrawCircle(width / 2, height / 2, radius, _errorPaint);
                }
            }
        }

        #endregion
    }
}
