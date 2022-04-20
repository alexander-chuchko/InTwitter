using System;
using Xamarin.Forms;

namespace InTwitter.Controls.Stories
{
    public class CustomLabel : Label
    {
        #region --- Public properties ---

        public event EventHandler<EventArgs> TextChanged;

        public static readonly new BindableProperty TextProperty =
            BindableProperty.Create(
                propertyName: nameof(Text),
                returnType: typeof(string),
                declaringType: typeof(CustomLabel),
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: TextChangedHandler);

        public new string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                base.Text = value;
                SetValue(TextProperty, value);
            }
        }

        #endregion

        #region --- Private helpers ---

        private static void TextChangedHandler(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomLabel customLabel)
            {
                customLabel.TextChanged?.Invoke(customLabel, new EventArgs());
            }
        }

        #endregion
    }
}
