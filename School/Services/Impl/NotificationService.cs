using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace School.Services.Impl
{
    public class NotificationService

    {

        public void sendNotification()
        {

            try
            {

                string applicationID = "AAAA1EfZ36A:APA91bErsR3LiWc-vLLigrFG0aMovC9jN3nChyAauxgqcgg_NEEqvkXEMoUJs7d5uHhlDQw5DdXxfjPCXHOmbWJw81yGCfSJcpbcXnxM0Iqm-qM1LEOQErneqEISsyDNoCF7VsW2IAQc";

                string senderId = "911738527648";

                string deviceId = "194f98004fbdc984";

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = "School app notifiaction",
                        title = "You have new grades!",
                        sound = "Enabled"

                    }
                };
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }



        }





    }
}
