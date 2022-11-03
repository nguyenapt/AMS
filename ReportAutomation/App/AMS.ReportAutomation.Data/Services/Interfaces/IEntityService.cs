using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Services.Interfaces
{
    public interface IEntityService<T> where T : class
    {
        void Add(T entity, bool isExecuteImmediate = false);
        void Delete(T entity, bool isExecuteImmediate = false);
        IEnumerable<T> GetAll();
        void Update(T entity, bool isExecuteImmediate = false);
        void Save();
    }
}
