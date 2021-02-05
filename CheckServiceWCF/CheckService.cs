using Microsoft.Extensions.Configuration;
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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICheckService
    {

        [OperationContract]
        [WebInvoke(
              Method = "POST",
              RequestFormat = WebMessageFormat.Json,
              ResponseFormat = WebMessageFormat.Json,
              UriTemplate = "/SendCheck",
              BodyStyle = WebMessageBodyStyle.WrappedRequest
               )]
       
        string SendCheck();

        //[OperationContract]
        //string RequestChecks(int id);

        // TODO: Add your service operations here
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

            Uri baseAddress = new Uri("http://localhost:5778");

            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(CheckService), baseAddress))
            {
               
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }

           

        }
    }

   
}
