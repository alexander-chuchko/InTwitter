using InTwitter.Mocks;
using InTwitter.Models.Icon;
using InTwitter.Models.Stories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace InTwitter.Extensions
{
    public static class UserStoryExtension
    {
        public static UserStoryViewModel ToUserStoryViewModelFromUserStories(this UserStories userStories, List<PostData> postDatas)
        {
            UserStoryViewModel userStoryViewModel = null;

            if (userStories != null)
            {
                userStoryViewModel = new UserStoryViewModel()
                {
                    Id = Guid.Parse(userStories.Id),
                    IsAuthorized = postDatas != null
                    ? false
                    : true,
                    UserIcon = UserState.Instance.Users.Where(u => u.Id == Guid.Parse(userStories.Id)).Select(s => s.IconSource).FirstOrDefault(),
                    IsLoaded = userStories.MediaStoriesSources.ToList().Count() > 0 ? true : false,
                };

                if (userStoryViewModel.IsAuthorized)
                {
                    userStoryViewModel.Name = "Add";

                    userStoryViewModel.OutlineСolor = userStoryViewModel.IsLoaded
                        ? Color.FromHex("#2356c5")
                        : Color.Transparent;
                }
                else
                {
                    bool result = postDatas.Exists(ui => ui.UserId == userStories.Id && userStories.MediaStoriesSources.ToList().Count != ui.LastMediaElement);
                    userStoryViewModel.OutlineСolor = result
                        ? Color.FromHex("#2356c5")
                        : Color.FromHex("#DEDFE1");
                    string name = UserState.Instance.Users.Where(u => u.Id == Guid.Parse(userStories.Id)).Select(s => s.Name).FirstOrDefault();
                    userStoryViewModel.Name = name;
                }
            }

            return userStoryViewModel;
        }

        public static UserStory ToUserStory(this UserStoryViewModel userStoryViewModel)
        {
            UserStory userStory = null;

            if (userStoryViewModel != null)
            {
                userStory = new UserStory
                {
                    Id = userStoryViewModel.Id,
                    IsAuthorized = userStoryViewModel.IsAuthorized,
                    IsLoaded = userStoryViewModel.IsLoaded,
                    Name = userStoryViewModel.Name,
                    OutlineСolor = userStoryViewModel.OutlineСolor,
                    UserIcon = userStoryViewModel.UserIcon,
                    UserId = userStoryViewModel._UserId,
                };
            }

            return userStory;
        }

        public static UserStoryViewModel ToUserStoryViewModel(this UserStory userStory)
        {
            UserStoryViewModel userStoryViewModel = null;

            if (userStory != null)
            {
                userStoryViewModel = new UserStoryViewModel
                {
                    Id = userStory.Id,
                    IsAuthorized = userStory.IsAuthorized,
                    IsLoaded = userStory.IsLoaded,
                    Name = userStory.Name,
                    OutlineСolor = userStory.OutlineСolor,
                    UserIcon = userStory.UserIcon,
                    _UserId = userStory.UserId,
                };
            }

            return userStoryViewModel;
        }
    }
}
