﻿using CheckServiceWCF.Entities;
using CheckServiceWCF.Handlers;
using CheckServiceWCF.Interface_Repository;
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


        static IRepository<CheckEntity> repository;

        static CheckService() 
        {
            repository = new SQLCheckRepository();
        }
        public void PostCheck()
        {
            string json = OperationContext.Current.RequestContext.RequestMessage.ToString();
            CheckEntity check = SerializeHandler.DeserializeFile<CheckEntity>(json);

            repository.SaveCheck(check);

            Console.WriteLine(String.Format("№" + ++filesCounter + " - Recieved Json "));
        }

        public string GetChecks(string size)
        {
            CheckEntityList checkList = null;
            if (Int32.TryParse(size, out int sizeInt))
            {
                checkList = new CheckEntityList();

                repository.GetLastNChecks(sizeInt);

                return SerializeHandler.SerializeMessage(checkList);
            }
            else
            {
                Logger.Log.Info(String.Format("Wrong id"));
            }

            Logger.Log.Info(String.Format("Last {0} Checks requested", sizeInt));
            Console.WriteLine(String.Format("Last {0} Checks requested", sizeInt));

            return size.ToString();
        }

    }
}
