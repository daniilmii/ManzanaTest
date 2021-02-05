using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CheckServiceWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CheckService : ICheckService
    {
        static int filesCounter = 0;

        public string SendCheck()
        {
           

            string JSONstring = OperationContext.Current.RequestContext.RequestMessage.ToString();

            Console.WriteLine(String.Format("№" + ++filesCounter + " - Recieved Json "));

            return JSONstring ;





            // return json;
        }
        //public string RequestChecks(int id) 
        //{
        //    Logger.Log.Info(String.Format("Checks with id  {0} requested", id));
        //    Console.WriteLine(String.Format("Checks with id  {0} requested", id));
        //    return id.ToString();
        //}

    }
}
