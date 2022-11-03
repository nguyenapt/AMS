using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AMS.ReportAutomation.Data.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        List<T> FindBy(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Use this when you have a complex SQL query on a large dataset that you will do further querying after Where condition, because it will return an incompleted SQL query execution.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> FindWhere(Expression<Func<T, bool>> predicate);
        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);
        long? FindMax(Expression<Func<T, bool>> predicate, Expression<Func<T, long?>> selector);

        T Add(T entity);
        T Delete(T entity);
        void Edit(T entity);
        void Save();
        void Add(IEnumerable<T> entities);
        void BatchDelete(Expression<Func<T, bool>> predicate);
    }
}