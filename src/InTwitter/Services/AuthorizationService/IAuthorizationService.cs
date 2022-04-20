using System;
using System.Threading.Tasks;
using InTwitter.Helpers;

namespace InTwitter.Services.AuthorizationService
{
    public interface IAuthorizationService
    {
        Guid CurrentUserId { get; set; }

        bool IsAuthorize { get; }

        Task<AOResult> IsUserExist(string email);
    }
}