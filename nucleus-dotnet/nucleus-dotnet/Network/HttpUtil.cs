using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nucleus.Network {
    public class HttpUtil {
        public static async Task<string> Post(string url, object data) {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync())) {
                string json = JsonConvert.SerializeObject(data);
                streamWriter.Write(json);
            }

            var httpResponse = await httpWebRequest.GetResponseAsync();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                string result = streamReader.ReadToEnd();
                return result;
            }
        }

        public static async Task<T> Post<T>(string url, object data) {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync())) {
                string json = JsonConvert.SerializeObject(data);
                streamWriter.Write(json);
            }

            var httpResponse = await httpWebRequest.GetResponseAsync();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                string result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(result);
            }
        }
    }
}
