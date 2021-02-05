using System;
using System.Collections.Generic;
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
        public string SendCheck(string check)
        {
           // Logger.Log.Info(String.Format("Check {0} recieved", check));
           // Console.WriteLine(String.Format("Check {0} recieved", check));
            return check;
        }
        //public string RequestChecks(int id) 
        //{
        //    Logger.Log.Info(String.Format("Checks with id  {0} requested", id));
        //    Console.WriteLine(String.Format("Checks with id  {0} requested", id));
        //    return id.ToString();
        //}

    }
}
