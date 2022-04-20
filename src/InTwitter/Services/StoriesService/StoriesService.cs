using InTwitter.Models.Stories;
using InTwitter.Services.AuthorizationService;
using InTwitter.Services.Repository;
using Realms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InTwitter.Services.StoriesService
{
    public class StoriesService : IStoriesService
    {
        private readonly IRepository _repository;
        private readonly IAuthorizationService _authorizationService;

        public StoriesService(IRepository repository, IAuthorizationService authorizationService)
        {
            this._repository = repository;
            this._authorizationService = authorizationService;
        }

        #region ---IStoriesService Implementation---

        public async Task<List<PostData>> GetIdsFollowers<T>()
            where T : UserStories, new()
        {
            List<PostData> postDataSubscribers = null;
            try
            {
                var storiesModel = await GetByIdStoriesModelAsync<T>(_authorizationService.CurrentUserId.ToString());
                postDataSubscribers = (List<PostData>)storiesModel.PostData.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return postDataSubscribers;
        }

        public async Task<bool> DeleteAllStoriesModelAsync<T>(IEnumerable<T> list)
            where T : RealmObject, new()
        {
            bool result = true;
            try
            {
                foreach (var obj in list)
                {
                    await _repository.DeleteAsync(obj);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<bool> DeleteStoriesModelAsync<T>(T entity)
            where T : RealmObject, new()
        {
            bool result = true;
            try
            {
                await _repository.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                result = false;
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<bool> DeleteObjectAndItsDependencies<T>(T entity)
            where T : UserStories, new()
        {
            bool result = true;
            try
            {
                await _repository.DeleteCollection(entity.PostData);
                await _repository.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                result = false;
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<bool> SaveStoriesModelAsync<T>(T entity, bool update = false)
            where T : RealmObject, new()
        {
            bool result = true;
            try
            {
                await _repository.InsertOrUpdateAsync(entity, update);
            }
            catch (Exception ex)
            {
                result = false;
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetAllStoriesModelAsync<T>()
            where T : RealmObject, new()
        {
            IEnumerable<T> storiesModel = null;
            try
            {
                storiesModel = await _repository.GetAllAsync<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return storiesModel;
        }

        public async Task<T> GetByIdStoriesModelAsync<T>(string id)
            where T : RealmObject, new()
        {
            T storiesModel = null;

            try
            {
                storiesModel = await _repository.GetEntityById<T>(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return storiesModel;
        }

        #endregion
    }
}
