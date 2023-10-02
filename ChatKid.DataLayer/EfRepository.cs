using ChatKid.DataLayer.EntityInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace ChatKid.DataLayer
{
    public class EfRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        private readonly IDBContext _dbContext;
        private DbSet<T> _entities;

        protected virtual DbSet<T> Entities => _entities ?? (_entities = _dbContext.Set<T>());

        public IQueryable<T> Table => Entities;
        public IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        public EfRepository(IDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(object id) => await Entities.FindAsync(id);

        public async Task<bool> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Add(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> InsertAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.AddRange(entities);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.UpdateRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Entities.RemoveRange(entities);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
