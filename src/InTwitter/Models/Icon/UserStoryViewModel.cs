using InTwitter.Models.Base;
using System;
using System.Drawing;

namespace InTwitter.Models.Icon
{
    public class UserStoryViewModel : EntityViewModelBase
    {
        public Guid _UserId;
        private Guid UserId
        {
            get => _UserId;
            set => SetProperty(ref _UserId, value);
        }

        private string _UserIcon;
        public string UserIcon
        {
            get => _UserIcon;
            set => SetProperty(ref _UserIcon, value);
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        private bool _IsLoaded;
        public bool IsLoaded
        {
            get => _IsLoaded;
            set => SetProperty(ref _IsLoaded, value);
        }

        private bool _IsIsAuthorized;
        public bool IsAuthorized
        {
            get => _IsIsAuthorized;
            set => SetProperty(ref _IsIsAuthorized, value);
        }

        private Color _OutlineСolor;
        public Color OutlineСolor
        {
            get => _OutlineСolor;
            set => SetProperty(ref _OutlineСolor, value);
        }
    }
}
