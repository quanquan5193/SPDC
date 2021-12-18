using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SPDC.Common
{
    public class NotificationHelper
    {
        public static void PushNotification(string message, string title, string deviceId)
        {
            string senderId = "1036736982722";
            string applicationID = "AAAA8WJW7sI:APA91bHYGqobyaKkZzA32ttIcKMlszbJppQ9Z4OONw0JYqA7Z7PDx6IAC7Cavi1bscPF3MkJeGAF3mcEzBmf4vDBxnLutBGHAPDOrkhuPSd4tEXFElriOic7BJXzyvpKdSpZlD0HW39r";

            //string deviceId = "fzCIDabxlPGLTAhFMbfIvw:APA91bFX0Y9mYbHbqdDYu2OszqVxDtRS7wybiCM-908YRy7qAtbzv8JoNcQHUNDfIlplKJNlcyJx8tSkXXD5TWEdTLeKsj4iusY1OYiRUGfGoYLlAPV_J2BFLJTVY4XEY8_B4Tgrgtbu";
            //string deviceId = "dFtWDvFsUtsPAti6nK-8wM:APA91bEKKc4W92G7W4kkE2Uu9_bZuoZ-HStiq3bXIv26cVrq_pNtIxYdKsuGTKConbNeSKL2i6ElMCGPMpPkz9fxrfGo479oAj1E-yn-ZDtblS5eKING7TgpojURz777r1ew9cWahXwq";

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

            tRequest.Method = "post";

            tRequest.ContentType = "application/json";

            var data = new

            {

                to = deviceId,

                notification = new

                {

                    body = message,

                    title = title,

                    icon = "icon-url"

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
    }
}
