namespace InTwitter.Resources.Styles
{
    using Xamarin.Forms;
    using InTwitter.Resources.Fonts;

    public partial class Styles
    {
        #region ---Constructros---

        public Styles()
        {
            this.InitializeComponent();

            // Add new style files here
            this.AddValues(new Values());

            this.AddValues(new FontSizes());

            this.AddValues(new Colors());

            this.AddValues(new TweetStyles());

            this.AddValues(new PageStyles());

            this.AddValues(new SearchStyles());
        }

        #endregion

        #region ---Private helpers---

        private void AddValues(ResourceDictionary resources)
        {
            foreach (var item in resources)
            {
                this.Add(item.Key, item.Value);
            }
        }

        #endregion

    }
}