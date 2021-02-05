using CheckServiceWCF.Entities;
using CheckServiceWCF.Handlers;
using CheckServiceWCF.Interface_Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace CheckServiceWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CheckService : ICheckService
    {
        static IRepository<CheckEntity> repository;

        static CheckService() 
        {
            repository = new SQLCheckRepository();
        }
        public void PostCheck()
        {
            string xml = OperationContext.Current.RequestContext.RequestMessage.ToString();
            XmlDocument doc = new XmlDocument();
            
            doc.LoadXml(xml);
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            
            CheckEntity check = SerializeHandler.DeserializeFile<CheckEntity>(json);

            repository.SaveCheck(check);

            
        }

        public string GetChecks(string count)
        {
            
            if (Int32.TryParse(count, out int n))
            {
                return SerializeHandler.SerializeMessage(repository.GetLastNChecks(n));
            }
            else
            {
                Logger.Log.Info("Count value is not correct");
            }

            Logger.Log.Info(String.Format("Last {0} checks recieved", n));
            

            return "";
        }

    }
}
