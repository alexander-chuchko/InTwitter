using System;
using System.Linq;
using System.Threading.Tasks;
using InTwitter.Models;
using InTwitter.Helpers;
using InTwitter.Mocks;
using System.Collections.Generic;
using InTwitter.Models.User;
using InTwitter.Services.UserService;

namespace InTwitter.Services.UserInteractionService
{
    public class UserInteractionService : IUserInteractionService
    {
        private readonly IUserService _userService;

        public UserInteractionService(IUserService userService)
        {
            _userService = userService;
        }

        #region ---IUserInteractionServiceImplementation---

        public async Task SetIsUserFollow(Guid youUserId, Guid followedUserId, bool isFollow)
        {
            var folower = FollowerState.Instance.Followers.Where(f => f.UserOnId == followedUserId && f.UserWhoId == youUserId).FirstOrDefault();

            if (isFollow == false && folower == null)
            {
                FollowerState.Instance.Followers.Add(new Follower()
                {
                    UserOnId = followedUserId,
                    UserWhoId = youUserId,
                });
            }
            else if (isFollow == true && folower != null)
            {
                FollowerState.Instance.Followers.Remove(folower);
            }
        }

        public async Task SetIsUserInBlacklist(Guid youUserId, Guid bannedUserId, bool isinBlacklist)
        {
            var banned = BanState.Instance.Bans.Where(f => f.UserOnId == bannedUserId && f.UserWhoId == youUserId).FirstOrDefault();

            if (isinBlacklist == false && banned == null)
            {
                BanState.Instance.Bans.Add(new Ban()
                {
                    UserOnId = bannedUserId,
                    UserWhoId = youUserId,
                });
            }
            else if (isinBlacklist == true && banned != null)
            {
                BanState.Instance.Bans.Remove(banned);
            }

            await SetIsUserFollow(youUserId, bannedUserId, !isinBlacklist);

            await SetIsUserFollow(bannedUserId, youUserId, !isinBlacklist);
        }

        public async Task SetIsUserMuted(Guid youUserId, Guid mutedUserId, bool isMuted)
        {
            var muted = MuteState.Instance.Mutes.Where(f => f.UserOnId == mutedUserId && f.UserWhoId == youUserId).FirstOrDefault();

            if (isMuted == false && muted == null)
            {
                MuteState.Instance.Mutes.Add(new Mute()
                {
                    UserOnId = mutedUserId,
                    UserWhoId = youUserId,
                });
            }
            else if (isMuted == true && muted != null)
            {
                MuteState.Instance.Mutes.Remove(muted);
            }

            await SetIsUserFollow(youUserId, mutedUserId, !isMuted);
        }

        public async Task<AOResult> GetIsUserMuted(Guid youUserId, Guid mutedUserId)
        {
            var result = new AOResult();

            var mutedUser = MuteState.Instance.Mutes.Where(m => m.UserOnId == mutedUserId && m.UserWhoId == youUserId).FirstOrDefault();

            if (mutedUser != null)
            {
                result.SetSuccess();
            }

            await Task.Delay(300);

            return result;
        }

        public async Task<AOResult<List<User>>> GetMutedUsersAsync(Guid youUserId)
        {
            var result = new AOResult<List<User>>();

            try
            {
                var mutedUsers = MuteState.Instance.Mutes.Where(m => m.UserWhoId == youUserId);

                List<Guid> guidList = new List<Guid>();

                foreach (var item in mutedUsers)
                {
                    guidList.Add(item.UserOnId);
                }

                var users = await _userService.GetUsersAsync(guidList);

                if (users.IsSuccess)
                {
                    result.SetSuccess(users.Result);
                }
            }
            catch (Exception ex)
            {
                result.SetError(nameof(UserInteractionService), "Something went wrong with GetMutedUsers");
            }

            return result;
        }

        public async Task<AOResult> GetIsUserInBlacklist(Guid youUserId, Guid bannedUserId)
        {
            var result = new AOResult();

            var mutedUser = BanState.Instance.Bans.Where(m => m.UserOnId == bannedUserId && m.UserWhoId == youUserId).FirstOrDefault();

            if (mutedUser != null)
            {
                result.SetSuccess();
            }

            await Task.Delay(300);

            return result;
        }

        public async Task<AOResult<List<User>>> GetUsersInBlacklistAsync(Guid youUserId)
        {
            var result = new AOResult<List<User>>();

            try
            {
                var blUsers = BanState.Instance.Bans.Where(m => m.UserWhoId == youUserId);

                List<Guid> guidList = new List<Guid>();

                foreach (var item in blUsers)
                {
                    guidList.Add(item.UserOnId);
                }

                var users = await _userService.GetUsersAsync(guidList);

                if (users.IsSuccess)
                {
                    result.SetSuccess(users.Result);
                }
            }
            catch (Exception ex)
            {
                result.SetError(nameof(UserInteractionService), "Something went wrong with GetMutedUsers!", ex);
            }

            return result;
        }

        #endregion
    }
}
