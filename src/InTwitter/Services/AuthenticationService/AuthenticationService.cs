using System;
using System.Linq;
using System.Threading.Tasks;
using InTwitter.Mocks;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.SettingsManager;
using InTwitter.Helpers;

namespace InTwitter.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private ISettingsManager _settingsManager;

        private IAuthorizationService _authorizationService;

        public AuthenticationService(ISettingsManager settingsManager, IAuthorizationService authorizationService)
        {
            this._settingsManager = settingsManager;
            this._authorizationService = authorizationService;
        }

        #region ---IAuthenticationService Implementation---

        public async Task<AOResult> ContinueSessionAsync(string token)
        {
            AOResult result = new AOResult();

            var user = UserState.Instance.Users.Where(u => u.SessionToken == token).FirstOrDefault();

            if (user != null)
            {
                this._authorizationService.CurrentUserId = user.Id;

                result.SetSuccess();
            }

            return result;
        }

        public void LogOut()
        {
            if (this._authorizationService.IsAuthorize == true)
            {
                this._authorizationService.CurrentUserId = Guid.Empty;
                this._settingsManager.SessionToken = Guid.Empty.ToString();
            }
        }

        public async Task<AOResult> SigInAsync(string email, string password)
        {
            AOResult result = new AOResult();

            var user = UserState.Instance.Users.Where(u => u.Email == email && u.HashPassword == password).FirstOrDefault();

            if (user != null)
            {
                user.SessionToken = Guid.NewGuid().ToString();

                this._settingsManager.SessionToken = user.SessionToken;
                this._authorizationService.CurrentUserId = user.Id;

                result.SetSuccess();
            }

            return result;
        }

        public async Task<AOResult> SigUpAsync(string name, string email, string password)
        {
            AOResult result = new AOResult();
            //var id = Guid.NewGuid(); I write code
            var id = Guid.Parse("29101eed-ef1d-4117-8abb-800d8f7aa57c");
            UserState.Instance.Users.Add(new Models.User.User()
            {
                Id = id,
                Name = name,
                Email = email,
                HashPassword = password,
                IconSource = "pic_profile_small.png",
                //"https://giiava.com/wp-content/uploads/2016/04/user-100x100.png",
            });

            MocksGenerator.AddFollowers(id);

            MocksGenerator.GenerateTweet(2, id);

            result.SetSuccess();

            return result;
        }

        public bool HasSessionToken()
        {
            return _settingsManager.SessionToken != Guid.Empty.ToString() && _settingsManager.SessionToken != string.Empty;
        }

        #endregion
    }
}
