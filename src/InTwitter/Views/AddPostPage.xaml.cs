using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
//using Android;
namespace InTwitter.Views
{
    public partial class AddPostPage : BaseContentPage
    {
        public AddPostPage()
        {
            InitializeComponent();
            //App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}
