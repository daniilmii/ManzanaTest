using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckServiceWCF.Interface_Repository
{
    interface IRepository<T> : IDisposable
        where T : class
    {
       
        void SaveCheck(T item);
        IEnumerable<T> GetLastNChecks(int n);

       
    }
}
