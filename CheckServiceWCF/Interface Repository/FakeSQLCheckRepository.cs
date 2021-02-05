using CheckServiceWCF.Entities;
using CheckServiceWCF.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace CheckServiceWCF.Interface_Repository
{
    public class FakeSQLCheckRepository : IRepository<CheckEntity>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CheckEntity> GetLastNChecks(int n)
        {
            throw new NotImplementedException();
        }

        public void SaveCheck(CheckEntity item)
        {
            string json = SerializeHandler.SerializeMessage(item);
            FileHandler.WriteFile(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", (DateTime.UtcNow).ToString("dd_MM-HH_mm_ss")+"Check.txt"), json);

        }
       
    }
}