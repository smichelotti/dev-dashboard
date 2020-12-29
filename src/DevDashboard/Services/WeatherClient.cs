using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DevDashboard.Services
{
    public class WeatherClient
    {
        private HttpClient http = new HttpClient();

        public async Task<Weather> GetWeather(string location)
        {
            var response = await this.http.GetAsync($"http://weather.service.msn.com/find.aspx?src=outlook&weadegreetype=F&culture=en-US&weasearchstr={location}");
            var xml = await response.Content.ReadAsStringAsync();
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            var curr = xDoc.GetElementsByTagName("current")[0];
            var fc = xDoc.GetElementsByTagName("forecast")[0];
            var weather = new Weather
            {
                CurrentTemp = curr.Attributes["temperature"].Value, // DateTime.Now.Second.ToString()// "21"
                Conditions = curr.Attributes["skytext"].Value,
                Humidity = curr.Attributes["humidity"].Value,
                Low = fc.Attributes["low"].Value,
                High = fc.Attributes["high"].Value
            };
            return weather;
        }
    }

    public class WeatherData
    {
        public Weather Weather { get; set; }
    }

    public class Weather
    {
        public string CurrentTemp { get; set; }
        public string Conditions { get; set; }
        public string Humidity { get; set; }
        public string Low { get; set; }
        public string High { get; set; }

    }
}
