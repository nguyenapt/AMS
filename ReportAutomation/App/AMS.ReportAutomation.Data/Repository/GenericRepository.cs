using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.Exceptions;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace AMS.ReportAutomation.Data.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
      where T : class
    {
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;
        protected readonly IAMSLogger _logger;

        protected GenericRepository(DbContext context, IAMSLogger logger)
        {
            _entities = context;
            _dbset = context.Set<T>();
            _logger = logger;
        }

        public virtual List<T> GetAll()
        {
            return _dbset.ToList();
        }

        public List<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required
                   , new TransactionOptions
                   {
                       //ReadUncommitted will avoid DB deadlock, here we only need to read the data
                       IsolationLevel = IsolationLevel.ReadUncommitted
                   }))
            {
                var retData = _dbset.Where(predicate).ToList();
                scope.Complete();
                return retData;
            }
        }

        public IQueryable<T> FindWhere(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        public T FindFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required
                   , new TransactionOptions
                   {
                       //ReadUncommitted will avoid DB deadlock, here we only need to read the data
                       IsolationLevel = IsolationLevel.ReadUncommitted
                   }))
            {
                var retData = _dbset.Where(predicate).FirstOrDefault();
                scope.Complete();
                return retData;
            }
        }

        public long? FindMax(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required
                   , new TransactionOptions
                   {
                       //ReadUncommitted will avoid DB deadlock, here we only need to read the data
                       IsolationLevel = IsolationLevel.ReadUncommitted
                   }))
            {
                var retData = _dbset.Where(predicate).Max(selector);
                scope.Complete();
                return retData;
            }
        }

        public virtual T Add(T entity)
        {
            return _dbset.Add(entity);
        }

        public virtual T Delete(T entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        public void Add(IEnumerable<T> entities)
        {
            this._entities.BulkInsert(entities);
        }

        protected virtual TResult SafeExecute<TResult>(Func<TResult> func) => this.Execute<TResult, System.Data.DataException, Exception>(_logger, func);

        /// <summary>
        /// Execute function with try/catch to throw the wrapper error if it happen
        /// </summary>
        /// <param name="action">The action need to be invoked</param>
        /// <exception cref="GesDbException"></exception>
        /// <exception cref="ArgumentNullException">Thrown when the action parameter is null</exception>
        protected virtual void SafeExecute(Action action) => this.Execute<System.Data.DataException, Exception>(_logger, action);

        public virtual void BatchDelete(Expression<Func<T, bool>> predicate)
        {
            EFBatchOperation.For(DbContext, DbContext.Set<T>())
                    .Where(predicate)
                    .Delete();
        }

        public DbContext DbContext => _entities;
    }
}