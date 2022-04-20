using Prism.Mvvm;
using Prism.Navigation;
using InTwitter.Resources.Localization;

namespace InTwitter.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IInitialize
    {
        public ViewModelBase(INavigationService navigationService)
        {
            this.NavigationService = navigationService;

            TextResources = new LocalizedResources(typeof(UITextResources));
        }

        protected INavigationService NavigationService { get; set; }

        public LocalizedResources TextResources { get; private set; }

        #region ---IInitialize---

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        #endregion

        #region ---INavigationAware Implementation---

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #endregion

    }
}
