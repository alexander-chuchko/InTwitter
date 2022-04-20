using Realms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InTwitter.Services.Repository
{
    public class Repository : IRepository
    {
        private Realm _context;
        public Repository()
        {
            _context = Realm.GetInstance();
        }

        public async Task DeleteAsync<T>(T entity)
            where T : RealmObject, new()
        {
            _context.Write(() =>
            {
                _context.Remove(entity);
                entity = null;
            });
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            where T : RealmObject, new()
        {
            return _context.All<T>();
        }

        public async Task<T> GetEntityById<T>(string id)
            where T : RealmObject, new()
        {
            return _context.Find<T>(id);
        }

        public async Task InsertOrUpdateAsync<T>(T value, bool update = false)
            where T : RealmObject, new()
        {
            _context.Write(() => _context.Add<T>(value, update));
        }

        public async Task DeleteAllByTypeAsync<T>()
            where T : RealmObject, new()
        {
            _context.Write(() => _context.RemoveAll<T>());
        }

        public async Task DeleteCollection<T>(IQueryable<T> collection)
            where T : RealmObject, new()
        {
           _context.Write(() => _context.RemoveRange<T>(collection));
        }
    }
}
