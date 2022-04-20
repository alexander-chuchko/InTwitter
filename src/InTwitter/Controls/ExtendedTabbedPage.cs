using InTwitter.Views;
using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace InTwitter.Controls
{
    public class ExtendedTabbedPage : TabbedPage
    {
        private int _oldPageIndex;
        public ExtendedTabbedPage()
        {
            CurrentPageChanged += ExtendedTabbedPage_CurrentPageChanged;
        }

        #region ---Public properties---

        public static readonly BindableProperty ChosenTabProperty =
            BindableProperty.Create(
                propertyName: nameof(ChosenTab),
                returnType: typeof(string),
                declaringType: typeof(ExtendedTabbedPage),
                defaultValue: default,
                defaultBindingMode: BindingMode.TwoWay);

        public string ChosenTab
        {
            get => (string)GetValue(ChosenTabProperty);
            set => SetValue(ChosenTabProperty, value);
        }

        #endregion

        #region ---Overrides---

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(ChosenTab))
            {
                foreach (var page in Children)
                {
                    if (ChosenTab.Equals(page.GetType().Name))
                    {
                        CurrentPage = page;
                    }
                }
            }
        }

        #endregion

        #region ---Private helpers--

        private void ExtendedTabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            ChosenTab = CurrentPage.GetType().Name;
            Children[_oldPageIndex].IconImageSource = (Children[_oldPageIndex] as BaseTabPage).UnselectedTabIcon;

            var currentPageIndex = Children.IndexOf(CurrentPage);
            CurrentPage.IconImageSource = (CurrentPage as BaseTabPage).SelectedTabIcon;

            _oldPageIndex = currentPageIndex;
        }

        #endregion
    }
}
