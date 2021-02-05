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
            WebRequest request = WebRequest.Create("http://localhost:5778/Welcome/sdfdsfsfsdf");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("Request to add numbers: ");
            Console.WriteLine("Request status: " + response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine("Response: \n" + responseFromServer);
            Console.ReadLine();
        }
    }
}
