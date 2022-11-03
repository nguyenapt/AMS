using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.Exceptions;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services
{
    public abstract class EntityService<T> : IEntityService<T>  where T : class
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IAMSLogger _logger;
        public EntityService(IGenericRepository<T> repository, IAMSLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public virtual void Add(T entity, bool isExecuteImmediate = false)
        {
            Guard.AgainstNullArgument(nameof(entity), entity);

            _repository.Add(entity);
            if (isExecuteImmediate)
            {
                this.Save();
            }
        }

        public virtual void Update(T entity, bool isExecuteImmediate = false)
        {
            Guard.AgainstNullArgument(nameof(entity), entity);

            _repository.Edit(entity);
            if (isExecuteImmediate)
            {
                this.Save();
            }
        }

        public virtual void Delete(T entity, bool isExecuteImmediate = false)
        {
            Guard.AgainstNullArgument(nameof(entity), entity);

            _repository.Delete(entity);
            if (isExecuteImmediate)
            {
                this.Save();
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.SafeExecute<IEnumerable<T>>(() => _repository.GetAll());
        }

        public virtual void Save()
        {
            SafeExecute(() => _repository.Save());           
        }

        /// <summary>
        /// Safe handle exception when 
        /// </summary>
        /// <param name="action"></param>
        protected virtual void SafeExecute(Action action)
        {
            Guard.AgainstNullArgument(nameof(action), action);
            Guard.AgainstNullArgumentProperty(nameof(action), "Method", action.Method);

            try
            {
                action();
            }
            catch (Exception ex){
                _logger?.Error(ex, $"Exception when invoke action {action.Method.Name} in {this.GetType().Name}.");

                // Re-throw exception
                throw new Exception($"Error when execute function {action.Method.Name}", ex);
            }
        }
        protected virtual TResult SafeExecute<TResult>(Func<TResult> func)
        {
            Guard.AgainstNullArgument(nameof(func), func);
            Guard.AgainstNullArgumentProperty(nameof(func), "Method", func.Method);

            try
            {
                return func();
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, $"Exception when invoke action {func.Method.Name} in {this.GetType().Name}.");

                // Re-throw exception
                throw new Exception($"Error when execute function {func.Method.Name}", ex);
            }
        }
    }
}
