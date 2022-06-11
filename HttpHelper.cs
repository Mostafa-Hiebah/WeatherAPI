using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;


namespace WeatherAPI
{
    public class HttpHelper
    {
        //convert response to Json
        public string ConvertToJson(string url)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            var response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            var json = reader.ReadToEnd();

            return json;
        }
    }
}
