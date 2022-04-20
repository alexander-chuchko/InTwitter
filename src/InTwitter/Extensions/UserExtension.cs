using InTwitter.Models.User;

namespace InTwitter.Extensions
{
    public static class UserExtension
    {
        public static UserViewModel ToUserViewModel(this User user)
        {
            var userViewModel = new UserViewModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                HashPassword = user.HashPassword,
                IconSource = user.IconSource,
                WallPapperSource = user.WallPaperSource,
                SessionToken = user.SessionToken,
            };

            return userViewModel;
        }
    }
}
