using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //   RequestFormat = WebMessageFormat.Json,
        //   ResponseFormat = WebMessageFormat.Json,
        //   UriTemplate = "/AddNewCustomer")]
        [OperationContract]
        [WebGet(UriTemplate = "/Welcome/{check}")]
        string SendCheck(string check);

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
                // Enable metadata publishing.
                //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                //smb.HttpGetEnabled = true;
                //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                //host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }

            //// Step 1: Create a URI to serve as the base address.
            //Uri baseAddress = new Uri("http://localhost:5778/CheckService");

            //// Step 2: Create a ServiceHost instance.
            //ServiceHost selfHost = new ServiceHost(typeof(CheckService), baseAddress);

            //try
            //{
            //    // Step 3: Add a service endpoint.
            //    selfHost.AddServiceEndpoint(typeof(ICheckService), new WSHttpBinding(), "CheckService");

            //    // Step 4: Enable metadata exchange.
            //    ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //    smb.HttpGetEnabled = true;
            //    selfHost.Description.Behaviors.Add(smb);

            //    // Step 5: Start the service.
            //    selfHost.Open();
            //    Console.WriteLine("The service is ready.");

            //    // Close the ServiceHost to stop the service.
            //    Console.WriteLine("Press <Enter> to terminate the service.");
            //    Console.WriteLine();
            //    Console.ReadLine();
            //    selfHost.Close();
            //}
            //catch (CommunicationException ce)
            //{
            //    Console.WriteLine("An exception occurred: {0}", ce.Message);
            //    selfHost.Abort();
            //}

        }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
