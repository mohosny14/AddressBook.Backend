using AdressBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Domain.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        TEntity Find(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int take, int skip);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int? take, int? skip,
            Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        Task<IEnumerable<TEntity>> FindAllAsync(string[] includes = null);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int skip, int take);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int? skip, int? take,
            Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        Task<TEntity> FindAllAsync(Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void Attach(TEntity entity);
        void AttachRange(IEnumerable<TEntity> entities);
        int Count();
        int Count(Expression<Func<TEntity, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria);

        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);
    }
}