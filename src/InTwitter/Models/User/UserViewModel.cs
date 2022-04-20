using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using InTwitter.Models.Base;

namespace InTwitter.Models.User
{
    public class UserViewModel : EntityViewModelBase
    {
        private Guid id;
        public Guid Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string hashPassword;
        public string HashPassword
        {
            get => hashPassword;
            set => SetProperty(ref hashPassword, value);
        }

        private string iconSource;
        public string IconSource
        {
            get => iconSource;
            set => SetProperty(ref iconSource, value);
        }

        private string wallPapperSource;
        public string WallPapperSource
        {
            get => wallPapperSource;
            set => SetProperty(ref wallPapperSource, value);
        }

        private string sessionToken;
        public string SessionToken
        {
            get => sessionToken;
            set => SetProperty(ref sessionToken, value);
        }
    }
}
