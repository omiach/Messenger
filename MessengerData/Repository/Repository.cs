﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using MessengerData.Extensions;
using Microsoft.Data.SqlClient;
using MessengerData.Providers;

namespace MessengerData.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public ApplicationDbContext GetDbContext()
        {
            return _context;
        }

        public EntityEntry<T> Add(T entity)
        {
            return _dbSet.Add(entity);
        }

        public async Task<EntityEntry<T>> AddAsync(T entity)
        {
            return await _dbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entity)
        {
            _dbSet.AddRange(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return;
        }

        public T? FirstOrDefault(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,  
            bool AsNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.FirstOrDefault();

        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool AsNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();

        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, 
            int pageIndex = 0,
            int pageSize = 20,
            bool AsNoTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (AsNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query =  orderBy(query);
            }

            query = query.Skip(pageIndex * pageSize).Take(pageSize);
            return query;
        }

        public EntityEntry<T> Remove(T entity)
        {
           return _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public EntityEntry<T> Update(T entity)
        {
            return _dbSet.Update(entity);
        }

        public void UpdateRange(T entity)
        {
            _dbSet.UpdateRange(entity);
        }

        public SaveResult Save()
        {
            try
            {
                _context.SaveChanges();
                return new SaveResult().SetResultTrue();
            }
            catch (DbUpdateException exeption)
            {
                var result = new SaveResult();
                if ((exeption.InnerException as SqlException)?.Number == 2601)
                {
                    result.ErrorMessage.Add("User provider. Save changes. Duplicate field");
                }

                if (result.ErrorMessage.Count() == 0)
                {
                    result.ErrorMessage.Add("User provider. Save changes. Unhandled db update exception");
                }

                return result;
            }
            catch
            {
                return new SaveResult("User provider. Save changes. Unhandled exception.");
            }

        }

        public async Task<SaveResult> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                return new SaveResult().SetResultTrue();
            }
            catch (DbUpdateException exeption)
            {
                var result = new SaveResult();
                if ((exeption.InnerException as SqlException)?.Number == 2601)
                {
                    result.ErrorMessage.Add("User provider. Save changes. Duplicate field");
                }

                if (result.ErrorMessage.Count() == 0)
                {
                    result.ErrorMessage.Add("User provider. Save changes. Unhandled db update exception");
                }

                return result;
            }
            catch
            {
                return new SaveResult("User provider. Save changes. Unhandled exception.");
            }

        }

    }
}
