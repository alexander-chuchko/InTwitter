using System;
using System.Linq;
using System.Threading.Tasks;
using InTwitter.Mocks;
using InTwitter.Helpers;

namespace InTwitter.Services.AuthorizationService
{
    public class AuthorizationService : IAuthorizationService
    {
        #region ---IAuthorizationImplementation---

        private static Guid _currentUserid = Guid.Empty;

        public Guid CurrentUserId
        {
            get => _currentUserid;
            set => _currentUserid = value;
        }

        public bool IsAuthorize
        {
            get => _currentUserid != Guid.Empty;
        }

        public async Task<AOResult> IsUserExist(string email)
        {
            var result = new AOResult();

            if (UserState.Instance.Users.Any(u => u.Email == email))
            {
                result.SetSuccess();
            }

            return result;
        }

        #endregion
    }
}