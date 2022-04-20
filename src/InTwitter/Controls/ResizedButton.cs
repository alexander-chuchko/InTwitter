using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public class ResizedButton : Button
    {
        #region --- Public properties ---

        public static readonly BindableProperty IsNormalButtonSizeProperty =
            BindableProperty.Create(
                propertyName: nameof(IsNormalButtonSize),
                returnType: typeof(bool),
                declaringType: typeof(ResizedButton),
                defaultValue: false,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsNormalButtonSize
        {
            get => (bool)GetValue(IsNormalButtonSizeProperty);
            set => SetValue(IsNormalButtonSizeProperty, value);
        }

        #endregion

        #region --- Overrides ---

        protected override async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsNormalButtonSize))
            {
                var width = (Parent as Layout).Width;
                var buttonPosition = (width / 2) - (WidthRequest / 2);
                if (width != -1)
                {
                    if (IsNormalButtonSize)
                    {
                        VerticalOptions = LayoutOptions.Start;
                        await Task.Delay(50);
                        var animation = new Animation(r => CornerRadius = (int)r, 0, HeightRequest / 2);
                        animation.Commit(this, "CornerRadiusAnimation");
                        await this.LayoutTo(new Rectangle(buttonPosition, this.Y, WidthRequest, this.Height));
                        HorizontalOptions = LayoutOptions.Center;
                    }
                    else
                    {
                        await Task.Delay(200);
                        VerticalOptions = LayoutOptions.EndAndExpand;
                        await Task.Delay(50);
                        var animation = new Animation(r => CornerRadius = (int)r, 20, 0);
                        animation.Commit(this, "CornerRadiusAnimation");
                        await this.LayoutTo(new Rectangle(0, this.Y, width, this.Height));
                        HorizontalOptions = LayoutOptions.Fill;
                    }
                }
            }
        }

        #endregion
    }
}
