using Realms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InTwitter.Services.Repository
{
    public interface IRepository
    {
        Task DeleteAsync<T>(T entity)
            where T : RealmObject, new();
        Task InsertOrUpdateAsync<T>(T entity, bool update = false)
            where T : RealmObject, new();
        Task<IEnumerable<T>> GetAllAsync<T>()
            where T : RealmObject, new();
        Task<T> GetEntityById<T>(string id)
            where T : RealmObject, new();
        Task DeleteAllByTypeAsync<T>()
            where T : RealmObject, new();
        Task DeleteCollection<T>(IQueryable<T> collection)
            where T : RealmObject, new();
    }
}
