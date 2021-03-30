using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Diray.Servise
{
    public class ParsWeather
    {

        HttpClient client = new HttpClient();

        readonly string key = "b391797caa6dd33e7aec0a7a027f7326";
        private string lat = "58.010259";
        private string lon = "56.234195";

        public string iconUrl = "";
        public string temp = "";

        public async Task Request()
        {
            HttpResponseMessage response = await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={key}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var jobject = JObject.Parse(responseBody);

            temp = ((int)Double.Parse(jobject["main"].SelectToken("temp").ToString()) - 273).ToString();
            iconUrl = @"C:\Програмирование\Проекты\Diray\Diray" + @$"\Icons\{jobject["weather"][0].SelectToken("icon")}.png";
        }

    }
}
