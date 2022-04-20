using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InTwitter.Controls
{
    public partial class AddButton : ClickableContentView
    {
        private static double y;
        private static double fullheight;

        public AddButton()
        {
            InitializeComponent();
            this.SizeChanged += OnSizeChanged;
        }

        #region -- Public properties --

        public static readonly BindableProperty ScrollStateProperty =
                              BindableProperty.Create(
                                                      nameof(ScrollState),
                                                      typeof(double),
                                                      typeof(MainNavBar),
                                                      defaultValue: default,
                                                      defaultBindingMode: BindingMode.TwoWay,
                                                      propertyChanged: ScrollStatePropertyChanged);

        public double ScrollState
        {
            get => (double)GetValue(ScrollStateProperty);
            set => SetValue(ScrollStateProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private static void ScrollStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            AddButton addButton = bindable as AddButton;
            if (addButton != null)
            {
                if ((double)newValue >= 0 && fullheight != 0)
                {
                    double offset = y + (double)oldValue - (double)newValue;
                    offset = Math.Min(offset, 0);
                    offset = Math.Max(offset, -52);

                    addButton.TranslationY = fullheight + (fullheight * (offset / 52));
                    y = offset;
                }
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            fullheight = Height + Margin.Bottom + 20;
            this.SizeChanged -= OnSizeChanged;
        }

        #endregion
    }
}