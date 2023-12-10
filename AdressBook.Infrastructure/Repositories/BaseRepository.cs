using AdressBook.Domain.Enums;
using AdressBook.Domain.IRepositories;
using AdressBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Infrastructure.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.SingleOrDefault(criteria);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).ToList();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int skip, int take)
        {
            return _context.Set<TEntity>().Where(criteria).Skip(skip).Take(take).ToList();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int? skip, int? take,
            Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(criteria);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> FindAllAsync(string[] includes = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int take, int skip)
        {
            return await _context.Set<TEntity>().Where(criteria).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int? take, int? skip,
            Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(criteria);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public TEntity Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            _context.Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public void Attach(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
        }

        public void AttachRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AttachRange(entities);
        }

        public int Count()
        {
            return _context.Set<TEntity>().Count();
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return _context.Set<TEntity>().Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return await _context.Set<TEntity>().CountAsync(criteria);
        }

        public async Task<TEntity> FindAllAsync(Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = "ASC")
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.FirstOrDefaultAsync();
        }

        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            return entities;
        }
    }
}