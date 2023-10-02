using ChatKid.DataLayer.EntityInterfaces;

namespace ChatKid.DataLayer
{
    public interface IGenericRepository<T> where T : class, IBaseEntity
    {
        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
        Task<T> GetByIdAsync(object id);
        Task<bool> InsertAsync(T entity);
        Task<bool> InsertAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateAsync(IEnumerable<T> entities);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(IEnumerable<T> entities);
    }
}
