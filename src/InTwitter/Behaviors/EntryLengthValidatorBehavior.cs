using InTwitter.Controls.Stories;
using System;
using Xamarin.Forms;

namespace InTwitter.Behaviors
{
    public class LabelLengthValidatorBehavior : Behavior<CustomLabel>
    {
        #region --- Public Properties ---

        public int MaxLength { get; set; }

        #endregion

        #region --- Overrides ---

        protected override void OnAttachedTo(CustomLabel bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += HandleTextChanged;
        }

        protected override void OnDetachingFrom(CustomLabel bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= HandleTextChanged;
        }

        #endregion

        #region --- Private Helpers ---

        private void HandleTextChanged(object sender, EventArgs e)
        {
            var label = (CustomLabel)sender;

            if (label != null)
            {
                if (label.Text.Length > this.MaxLength)
                {
                    label.Text = string.Concat(label.Text.Substring(0, label.Text.Length - (label.Text.Length - this.MaxLength)), "...");
                }
                else
                {
                    label.Text = label.Text;
                }
            }
        }

        #endregion
    }
}
