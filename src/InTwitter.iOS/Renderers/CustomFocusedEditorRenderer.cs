using Foundation;
using InTwitter.Controls.AddPost;
using InTwitter.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FocusedEditor), typeof(CustomFocusedEditorRenderer))]
namespace InTwitter.iOS.Renderers
{
    class CustomFocusedEditorRenderer : EditorRenderer
    {

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
                var element = Element as FocusedEditor;

                try
                {
                    if (e.PropertyName == nameof(Element.Text))
                    {

                        var spanIndex = element.SpanIndex;

                        if (element.Text.Length > spanIndex)
                        {

                            var selectedRange = Control.SelectedRange;

                            var formatedString = $"<pre><span style=\"color: #02060e;font-size:17px;line-height:1.25;font-family:Ubuntu;font-weight:400;\">" +
                                $"{Control.Text.Substring(0, spanIndex)}" +
                                $"</span>" +
                                $"<span style=\"color: #F44336;font-size:17px;font-family:Ubuntu;line-height:1.25;font-weight:400;\">" +
                                $"{Control.Text.Substring(spanIndex, Control.Text.Length - spanIndex)}" +
                                $"</span></pre>";

                            var options = new NSAttributedStringDocumentAttributes();

                            options.DocumentType = NSDocumentType.HTML;

                            var error = new NSError();

                            Control.AttributedText = new NSAttributedString(formatedString, options, ref error);

                            if (error != null)
                            {
                            }

                            Control.SelectedRange = selectedRange;
                        }
                        else
                        {
                            var selectedRange = Control.SelectedRange;

                            var formatedString = "";

                            if (Element.Text.Length != 0)
                            {
                                formatedString = $"<pre><span style=\"color: #02060e;" +
                                                 $"font-size:17px;" +
                                                 $"font-family:Ubuntu;" +
                                                 $"font-weight:400;" +
                                                 $"line-height:1.25;\">" +
                                                 $"{Control.Text}" +
                                                 $"</span></pre>";
                            }


                            var options = new NSAttributedStringDocumentAttributes();

                            options.DocumentType = NSDocumentType.HTML;

                            var error = new NSError();

                            Control.AttributedText = new NSAttributedString(formatedString, options, ref error);

                            if (error != null)
                            {
                            }

                            Control.SelectedRange = selectedRange;
                        }

                    }
                }
                catch (Exception ex)
                {
                }


            }
        }
    }
}