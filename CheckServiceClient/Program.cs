using CheckServiceWCF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //WebRequest request = WebRequest.Create("http://localhost:5778/SendCheck/requeststring");


            var request = (HttpWebRequest)WebRequest.Create("http://localhost:5778/SendCheck");
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{\"user\":\"test\"," +
                              "\"password\":\"bla\"}";

                streamWriter.Write(json);
            }
            var response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine("Request to add numbers: ");
            Console.WriteLine("Request status: " + response.StatusDescription);

          
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine("Response: \n" + result);
                Console.ReadLine();
            }


            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          
            //Stream dataStream = response.GetResponseStream();
            //StreamReader reader = new StreamReader(dataStream);
            //string responseFromServer = reader.ReadToEnd();
           
        }
    }
}
