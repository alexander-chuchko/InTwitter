using System;
using System.Threading.Tasks;
using InTwitter.Mocks;
using System.Linq;
using InTwitter.Helpers;
using System.Collections.Generic;
using InTwitter.Models.Icon;

namespace InTwitter.Services.UserService
{
    public class UserService : IUserService
    {
        #region ---IUserService Implementation---

        public async Task<AOResult> ChangeEmailAsync(Guid userId, string email)
        {
            var result = new AOResult();
            var user = UserState.Instance.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                var index = UserState.Instance.Users.IndexOf(user);

                if (index != -1)
                {
                    UserState.Instance.Users[index].Email = email;
                }
            }

            result.SetSuccess();

            return result;
        }

        public async Task ChangeIconAsync(Guid userId, string iconSource)
        {
            var user = UserState.Instance.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                var index = UserState.Instance.Users.IndexOf(user);

                if (index != -1)
                {
                    UserState.Instance.Users[index].IconSource = iconSource;
                }
            }
        }

        public async Task ChangeNameAsync(Guid userId, string name)
        {
            var user = UserState.Instance.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                var index = UserState.Instance.Users.IndexOf(user);

                if (index != -1)
                {
                    UserState.Instance.Users[index].Name = name;
                }
            }
        }

        public async Task<AOResult> ChangePasswordAsync(Guid userId, string newPassword)
        {
            var result = new AOResult();
            var user = UserState.Instance.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                var index = UserState.Instance.Users.IndexOf(user);

                if (index != -1)
                {
                    UserState.Instance.Users[index].HashPassword = newPassword;
                }
            }

            result.SetSuccess();

            return result;
        }

        public async Task ChangeWallPaperAsync(Guid userId, string wallpaperSource)
        {
            var user = UserState.Instance.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                var index = UserState.Instance.Users.IndexOf(user);

                if (index != -1)
                {
                    UserState.Instance.Users[index].WallPaperSource = wallpaperSource;
                }
            }
        }

        public async Task<string> GetNameAsync(Guid id)
        {
            string name = string.Empty;
            var selectedName = UserState.Instance.Users.Where(u => u.Id == id).Select(s => s.Name).FirstOrDefault();

            if (!string.IsNullOrEmpty(selectedName))
            {
                name = selectedName;
            }

            return name;
        }

        public async Task<AOResult<Models.User.User>> GetUserAsync(Guid userId)
        {
            var result = new AOResult<Models.User.User>();
            var user = UserState.Instance.Users.FirstOrDefault(u => u.Id == userId);
            result.SetSuccess(user);
            return result;
        }

        public async Task<AOResult<List<Models.User.User>>> GetUsersAsync(List<Guid> userIds)
        {
            var result = new AOResult<List<Models.User.User>>();
            var users = new List<Models.User.User>();
            foreach (var id in userIds)
            {
                var user = UserState.Instance.Users.FirstOrDefault(u => u.Id == id);

                if (user != null)
                {
                    users.Add(user);
                }
            }

            result.SetSuccess(users);
            return result;
        }
        #endregion
    }
}
