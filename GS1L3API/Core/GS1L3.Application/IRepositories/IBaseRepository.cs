using GS1L3.Domain.Entities;
using System.Linq.Expressions;

namespace GS1L3.Application.IRepositories
{
    public interface IBaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);
        bool Remove(T model);
        Task<bool> RemoveAsync(string id);
        bool Update(T model);
        bool UpdateRange(List<T> datas);
        Task<int> SaveAsync();

        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetByIdAsync(string id, bool tracking = true);
        IQueryable<TResult> Select<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector);
    }
}

