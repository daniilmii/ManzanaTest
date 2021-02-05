using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CheckServiceWCF.Handlers
{
    static class RequestHandler
    {
        public static string SendRequest(string hostIp, string hostPort, string urlMethod, object body)
        {
            var request = (HttpWebRequest)WebRequest.Create(String.Format("http://" + hostIp + ':' + hostPort + urlMethod));
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var res = SerializeHandler.SerializeMessage(body);
                streamWriter.Write(res);
            }   
            var response = (HttpWebResponse)request.GetResponse();

            Logger.Log.Info("Request to add numbers: ");
            Logger.Log.Info("Request status: " + response.StatusDescription);


            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Logger.Log.Info("Response: \n" + result);
                return result;
            }
        }
    }
}
