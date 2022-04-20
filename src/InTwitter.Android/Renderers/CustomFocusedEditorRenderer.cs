using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using InTwitter.Controls.AddPost;
using InTwitter.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FocusedEditor), typeof(CustomFocusedEditorRenderer))]
namespace InTwitter.Droid.Renderers
{
    class CustomFocusedEditorRenderer : EditorRenderer
    {
        private bool withSpan;

        public CustomFocusedEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;
                Control.SetLineSpacing(1, 1.25f);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            FocusedEditor focusedEditor = sender as FocusedEditor;

            if (Control != null)
            {
                var index = Control.SelectionStart;

                SpannableString spannable = new SpannableString(Control.Text);
                if (Control.Text.Length > focusedEditor.SpanIndex)
                {
                    spannable.SetSpan(new ForegroundColorSpan(focusedEditor.SecondTextColor.ToAndroid()), focusedEditor.SpanIndex, Control.Text.Length, SpanTypes.ExclusiveExclusive);
                    
                    Control.TextFormatted = spannable;
                    Control.SetSelection(index);

                    withSpan = true;
                }
                if (Control.Text.Length <= focusedEditor.SpanIndex && withSpan)
                {
                    Control.TextFormatted = spannable;
                    Control.SetSelection(index);

                    withSpan = false;
                }
                
            }
        }
    }
}