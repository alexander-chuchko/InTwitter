using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;
using System;
using Xamarin.Forms;

namespace InTwitter.Behaviors
{
    public class TabbedPageNavigationBehavior : BehaviorBase<TabbedPage>
    {
        private Page _currentPage;

        #region --- Overrides ---

        protected override void OnAttachedTo(TabbedPage bindable)
        {
            bindable.CurrentPageChanged += OnCurrentPageChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(TabbedPage bindable)
        {
            bindable.CurrentPageChanged -= OnCurrentPageChanged;
            base.OnDetachingFrom(bindable);
        }

        #endregion

        #region --- Private helpers ---

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            var newPage = AssociatedObject.CurrentPage;

            if (_currentPage != null)
            {
                var parameters = new NavigationParameters();
                PageUtilities.OnNavigatedFrom(_currentPage, parameters);
                PageUtilities.OnNavigatedTo(newPage, parameters);
            }

            _currentPage = newPage;
        }

        #endregion
    }
}
