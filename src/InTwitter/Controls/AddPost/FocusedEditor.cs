using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InTwitter.Controls.AddPost
{
    public class FocusedEditor : Editor
    {
        public FocusedEditor()
        {
            this.SizeChanged += OnSizeChanged;
        }

        #region -- Public Properties --

        public static readonly BindableProperty SecondTextColorProperty =
                              BindableProperty.Create(
                                                      nameof(SecondTextColor),
                                                      typeof(Color),
                                                      typeof(FocusedEditor),
                                                      defaultValue: default,
                                                      defaultBindingMode: BindingMode.TwoWay);

        public Color SecondTextColor
        {
            get => (Color)GetValue(SecondTextColorProperty);
            set => SetValue(SecondTextColorProperty, value);
        }

        public static readonly BindableProperty SpanIndexProperty =
                              BindableProperty.Create(
                                                      nameof(SpanIndex),
                                                      typeof(int),
                                                      typeof(FocusedEditor),
                                                      defaultValue: default,
                                                      defaultBindingMode: BindingMode.TwoWay);

        public int SpanIndex
        {
            get => (int)GetValue(SpanIndexProperty);
            set => SetValue(SpanIndexProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private async void OnSizeChanged(object sender, EventArgs e)
        {
            await Task.Delay(50);
            this.Focus();
            this.SizeChanged -= OnSizeChanged;
        }

        #endregion
    }
}