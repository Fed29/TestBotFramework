using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Bot_With_Luis.Model
{

    public class SensorLuis
    {
        public string query { get; set; }
        public Intent[] intents { get; set; }
        public Entity[] entities { get; set; }

        public static async Task<SensorLuis> ParseUserInput(string strInput)
        {
            string strRet = string.Empty;
            string strInpuEscaped = Uri.EscapeDataString(strInput);
            string uri = "https://api.projectoxford.ai/luis/v1/application?id=f720deed-314c-4e58-8453-613bbd6c1c0a&subscription-key=59e68281926d4afe9c237f69406fae76&q=";

            using (var client = new HttpClient())
            {
                HttpResponseMessage msg = await client.GetAsync(uri + strInpuEscaped);
                if(msg.IsSuccessStatusCode)
                {
                    var jsonResponse = await msg.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<SensorLuis>(jsonResponse);
                }
            }

            return null;
        }
    }

    public class Intent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }

}