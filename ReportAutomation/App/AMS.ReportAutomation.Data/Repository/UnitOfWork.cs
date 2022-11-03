using AMS.ReportAutomation.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Repository
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext, new()
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        private TContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Configuration.AutoDetectChangesEnabled = false;
            _dbContext.Configuration.ValidateOnSaveEnabled = false;
        }


        public TContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public int Commit()
        {
            try
            {
                // Save changes with the default options
                return _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                // Write log
                throw new Exception("Can not be commit the changes. Check inner exception for detail.", ex);
            }
        }

        public void Dispose()
        {

        }
    }
}
