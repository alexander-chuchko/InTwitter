using InTwitter.Helpers;
using InTwitter.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTwitter.Services.UserInteractionService
{
    public interface IUserInteractionService
    {
        Task SetIsUserFollow(Guid youUserId, Guid followedUserId, bool isFollow);

        Task<AOResult> GetIsUserMuted(Guid youUserId, Guid mutedUserId);

        Task<AOResult<List<User>>> GetMutedUsersAsync(Guid youUserId);

        Task SetIsUserMuted(Guid youUserId, Guid mutedUserId, bool isMuted);

        Task<AOResult> GetIsUserInBlacklist(Guid youUserId, Guid bannedUserId);

        Task<AOResult<List<User>>> GetUsersInBlacklistAsync(Guid youUserId);

        Task SetIsUserInBlacklist(Guid youUserId, Guid bannedUserId, bool isinBlacklist);
    }
}
