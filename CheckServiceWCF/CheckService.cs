﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;

namespace CheckServiceWCF
{
   
    [ServiceContract]
    public interface ICheckService
    {

        [OperationContract]
        [WebInvoke(
              Method = "POST",
              RequestFormat = WebMessageFormat.Json,
              ResponseFormat = WebMessageFormat.Json,
              UriTemplate = "/PostCheck",
              BodyStyle = WebMessageBodyStyle.WrappedRequest
               )]
       
        void PostCheck();

        [OperationContract]
        [WebGet(UriTemplate ="/GetChecks/{size}" )]
        string GetChecks(string size);

        
    }

    class Program
    {
        static void Main(string[] args)
        {


            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("conf.json", optional: true)
            .Build();
            Logger.InitLogger();
            
            Configurations.Configuration = configuration;
            Configurations.ConfigLoader();
          
            Uri baseAddress = new Uri(String.Format("http://{0}:{1}", Configurations.CurrentConfig.HostIp, Configurations.CurrentConfig.HostPort));

            
            using (ServiceHost host = new ServiceHost(typeof(CheckService), baseAddress))
            {
               
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

               
                host.Close();
            }

           

        }
    }

   
}
