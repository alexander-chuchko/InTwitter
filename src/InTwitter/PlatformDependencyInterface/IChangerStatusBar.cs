namespace InTwitter.PlatformDependencyInterface
{
    public interface IChangerStatusBar
    {
        void ChangeStatusBarColor(Xamarin.Forms.Color color);

        void ChangeTitleColor(bool isLight);
    }
}