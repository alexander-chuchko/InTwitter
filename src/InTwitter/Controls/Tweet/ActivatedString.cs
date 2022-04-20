namespace InTwitter.Controls
{
    public struct ActivatedString
    {
        public string Text;

        public bool IsActivated;

        public ActivatedString(string text, bool isActivated)
        {
            Text = text;
            IsActivated = isActivated;
        }
    }
}