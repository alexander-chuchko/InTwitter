using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public class LabelWithHashtags : DistributedLabel
    {
        public LabelWithHashtags()
            : base()
        {
        }

        public Style HashtagStyle
        {
            get => (Style)GetValue(HashtagStyleProperty);
            set => SetValue(HashtagStyleProperty, value);
        }

        public static BindableProperty HashtagStyleProperty = BindableProperty.Create(
            nameof(HashtagStyle),
            typeof(Style),
            typeof(DistributedLabel));

        public ICommand HashtagTapCommand
        {
            get => (ICommand)GetValue(HashtagTapCommandProperty);
            set => SetValue(HashtagTapCommandProperty, value);
        }

        public static BindableProperty HashtagTapCommandProperty = BindableProperty.Create(
            nameof(HashtagTapCommand),
            typeof(ICommand),
            typeof(DistributedLabel));

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

            var indexes = GetIndexesOfSymbol(text, '#');

            if (indexes.Count != 0)
            {
                text = GetPreparedText(text, indexes);
            }

            var words = text.Split(' ');

            foreach (var word in words)
            {
                var label = IsHashTag(word) ? GetHashtagLabel(word + " ") : GetSimpleLabel(word + " ");

                this.Children.Add(label);
            }

            if (HasShorted)
            {
                this.Children.Add(GetSimpleLabel("..."));
                this.Children.Add(GetMoreTextLabel("more"));
            }
        }

        protected bool IsHashTag(string word)
        {
            return Regex.IsMatch(word, @"(\#[a-zA-Z_]+\b)(?!;)");
        }

        protected Label GetHashtagLabel(string word)
        {
            var label = GetSimpleLabel(word);

            label.Style = HashtagStyle;

            var tapRecognizer = new TapGestureRecognizer()
            {
                Command = HashtagTapCommand,
                CommandParameter = word,
            };

            label.GestureRecognizers.Add(tapRecognizer);

            return label;
        }

        protected string GetPreparedText(string text, List<int> indexes)
        {
            var builder = new StringBuilder(text);

            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                builder.Insert(indexes[i], ' ');
            }

            return builder.ToString();
        }

        protected List<int> GetIndexesOfSymbol(string text, char symbol)
        {
            var indexes = new List<int>();

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i] == symbol && text[i - 1] != ' ')
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }
    }
}