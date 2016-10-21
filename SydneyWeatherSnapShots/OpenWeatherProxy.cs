using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jobs
{
    public static class TemperatureExtension
    {
        public static double KevinToCelsius(this double kevin)
        {
            return kevin - 273.15;
        }
    }

    public class OpenWeatherProxy
    {
        public static async Task<OpenWeatherResult> FetchSydney()
        {
            //{"_id":7839759,"name":"North Sydney","country":"AU","coord":{"lon":151.21019,"lat":-33.834221}}
            //{"_id":6619279,"name":"City of Sydney","country":"AU","coord":{"lon":151.208435,"lat":-33.867779}}
            const string BASE_URL = "http://api.openweathermap.org/data/2.5/weather?APPID=be357b55d043cf450620449e0e8b4d7c&id=6619279&units=metric";
            HttpClient client = new HttpClient();
            var result  = await client.GetAsync(BASE_URL);
            string strReslt = await result.Content.ReadAsStringAsync();
            return Parse(strReslt);
        }

        private static OpenWeatherResult Parse(string result)
        {
            var serializer = new DataContractJsonSerializer(typeof(OpenWeatherResult));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(result)))
            {
                var weather = (OpenWeatherResult) serializer.ReadObject(stream);
                return weather;
            }
        }
    }
}
