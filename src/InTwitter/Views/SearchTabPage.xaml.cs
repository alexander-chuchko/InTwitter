using InTwitter.Controls;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace InTwitter.Views
{
    public partial class SearchTabPage : BaseTabPage
    {
        public SearchTabPage()
        {
            InitializeComponent();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}
