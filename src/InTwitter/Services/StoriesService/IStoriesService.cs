using InTwitter.Models.Stories;
using Realms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTwitter.Services.StoriesService
{
    public interface IStoriesService
    {
        public Task<List<PostData>> GetIdsFollowers<T>()
            where T : UserStories, new();
        Task<bool> SaveStoriesModelAsync<T>(T entity, bool update = false)
            where T : RealmObject, new();
        Task<bool> DeleteStoriesModelAsync<T>(T entity)
            where T : RealmObject, new();
        Task<IEnumerable<T>> GetAllStoriesModelAsync<T>()
            where T : RealmObject, new();
        Task<T> GetByIdStoriesModelAsync<T>(string id)
            where T : RealmObject, new();
        Task<bool> DeleteAllStoriesModelAsync<T>(IEnumerable<T> list)
            where T : RealmObject, new();
        Task<bool> DeleteObjectAndItsDependencies<T>(T entity)
            where T : UserStories, new();
    }
}
