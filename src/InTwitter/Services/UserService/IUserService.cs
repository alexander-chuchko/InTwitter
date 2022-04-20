using InTwitter.Models.User;
using System;
using System.Threading.Tasks;
using InTwitter.Helpers;
using System.Collections.Generic;
using InTwitter.Models.Icon;

namespace InTwitter.Services.UserService
{
    public interface IUserService
    {
        Task<AOResult<User>> GetUserAsync(Guid userId);

        Task<AOResult<List<User>>> GetUsersAsync(List<Guid> userIds);

        Task ChangeNameAsync(Guid userId, string name);

        Task<AOResult> ChangeEmailAsync(Guid userId, string email);

        Task<AOResult> ChangePasswordAsync(Guid userId, string newPassword);

        Task ChangeIconAsync(Guid userId, string iconSource);

        Task ChangeWallPaperAsync(Guid userId, string wallpaperSource);

        Task<string> GetNameAsync(Guid userId);
    }
}
