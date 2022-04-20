using InTwitter.Models;
using System;
using System.Threading.Tasks;
using InTwitter.Helpers;

namespace InTwitter.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<AOResult> SigInAsync(string email, string password);

        Task<AOResult> SigUpAsync(string name, string email, string password);

        Task<AOResult> ContinueSessionAsync(string token);

        bool HasSessionToken();

        void LogOut();
    }
}
