using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace InTwitter.Controls
{
    public class LabelWithAccentedWords : LabelWithHashtags
    {
        public string AccentedText
        {
            get => (string)GetValue(AccentedTextProperty);
            set => SetValue(AccentedTextProperty, value);
        }

        public static BindableProperty AccentedTextProperty = BindableProperty.Create(
            nameof(AccentedText),
            typeof(string),
            typeof(LabelWithAccentedWords),
            defaultValue: string.Empty,
            propertyChanged: OnAccentedTextTextPropertyChanged);

        protected override void UpdateText(string text)
        {
            if (Text == null)
            {
                return;
            }

            if (IsShorted == true && Text.Length > MaxSymbols)
            {
                text = Text?.Substring(0, MaxSymbols == 0 ? Text.Length : MaxSymbols);
                HasShorted = true;
            }
            else
            {
                text = Text;
                HasShorted = false;
            }

            this.Children.Clear();

            var hashtagIndexes = GetIndexesOfSymbol(text, '#');

            if (hashtagIndexes.Count != 0)
            {
                text = GetPreparedText(text, hashtagIndexes);
            }

            var indexes = AccentedText.StartsWith('#') ? new List<(int, int)>() : GetIndexes(text?.ToLower(), AccentedText?.ToLower());

            List<ActivatedString> texts = new List<ActivatedString>();
            StringBuilder str = new StringBuilder();
            int counter = 0;

            for (int i = 0; i < text.Length; i++)
            {
                str.Append(text[i]);

                if (indexes.Count > counter)
                {
                    if (indexes[counter].Item1 == i + 1)
                    {
                        texts.Add(new ActivatedString(str.ToString(), false));
                        str.Clear();
                    }
                    else if ((indexes[counter].Item1 + indexes[counter].Item2) == i + 1)
                    {
                        texts.Add(new ActivatedString(str.ToString(), true));
                        str.Clear();
                        counter++;
                    }
                }
            }

            if (str.Length > 0)
            {
                texts.Add(new ActivatedString(str.ToString(), false));
            }

            var words = new List<ActivatedString>();
            foreach (var item in texts)
            {
                var source = item.Text.Split(' ');
                for (int i = 0; i < source.Length; i++)
                {
                    if ((i + 1) == source.Length)
                    {
                        words.Add(new ActivatedString(source[i], item.IsActivated));
                    }
                    else
                    {
                        words.Add(new ActivatedString(source[i] + " ", item.IsActivated));
                    }
                }
            }

            foreach (var word in words)
            {
                Label label;
                if (word.IsActivated)
                {
                    label = GetAccentedLabel(word.Text);
                }
                else if (IsHashTag(word.Text))
                {
                    label = GetHashtagLabel(word.Text + " ");
                }
                else
                {
                    label = GetSimpleLabel(word.Text);
                }

                this.Children.Add(label);
            }

            if (HasShorted)
            {
                this.Children.Add(GetSimpleLabel("..."));
                this.Children.Add(GetMoreTextLabel("more"));
            }
        }

        private static void OnAccentedTextTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private Label GetAccentedLabel(string text)
        {
            var label = GetSimpleLabel(text);

            label.BackgroundColor = Color.FromHex("#C7D6F7");

            return label;
        }

        private List<(int, int)> GetIndexes(string text, string sub)
        {
            var result = new List<(int, int)>();

            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(sub))
            {
                var index = 0;

                while (index != -1)
                {
                    index = text.IndexOf(sub, index);

                    if (index != -1)
                    {
                        result.Add((index, sub.Length));
                        index++;
                    }
                }
            }

            return result;
        }
    }
}