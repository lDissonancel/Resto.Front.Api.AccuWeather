using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Net;

namespace Resto.Front.Api.AccuWeather.ConfigAWeather
{
    public class AWeatherConfig
    {
        private readonly bool Details;
        private readonly bool Metric;
        private readonly Uri BaseURL;
        private readonly string ApiKey; 
        private readonly string Language; 
        private readonly string IpAddr; 
        private readonly int LocationKey; 

        public AWeatherConfig()
        {
            BaseURL = new Uri("http://dataservice.accuweather.com/");
            Details = false;
            Metric = true;
            ApiKey = GetApiKey();
            Language = GetLanguage();
            IpAddr = GetIpV4();
            LocationKey = GetLocationKey();
        }
        private static AWeatherVariables GetCityFromJson(string json) => JsonConvert.DeserializeObject<AWeatherVariables>(json);

        public string GetCurrentConditions()
        {
            return BaseURL + $"currentconditions/v1/{LocationKey}?apikey={ApiKey}&language={Language}&details={Details}";
        }

        public string GetWeatherFor1Hour()
        {
            return BaseURL + $"forecasts/v1/hourly/1hour/{LocationKey}?apikey={ApiKey}&language={Language}&details={Details}&metric={Metric}";
        }

        public string GetWeatherFor12Hours()
        {
            return BaseURL + $"forecasts/v1/hourly/12hour/{LocationKey}?apikey={ApiKey}&language={Language}&details={Details}&metric={Metric}";
        }

        private string SearchCity()
        {
            return BaseURL + $"locations/v1/cities/ipaddress?apikey={ApiKey}&q={IpAddr}&language=:{Language}";
        }

        private string GetIpV4() // Получаем наш внешний IP
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string ipV4 = ConfigurationManager.AppSettings.Get("IPv4");

            if(ipV4 != null && ipV4 != string.Empty && ipV4 != "127.0.0.1" && ipV4 != "http://localhost" &&  ipV4 != "0")
            {
                return ipV4;
            }
            else
            {
                config.AppSettings.Settings["IPv4"].Value = new WebClient().DownloadString("http://icanhazip.com/").Replace("\n", string.Empty);
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                ipV4 = ConfigurationManager.AppSettings.Get("IPv4");

                return ipV4;
            }
        }

        private string GetLanguage() // Проверяем, проставлен ли язык в конфиге
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string language = ConfigurationManager.AppSettings.Get("Language");

            if (language != null && language != string.Empty && language != "0")
            {
                return language;
            }
            else
            {
                config.AppSettings.Settings["Language"].Value = "ru-Ru";
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                language = ConfigurationManager.AppSettings.Get("Language");

                return language;
            }
        }
        private string GetApiKey() // Проверяем наличие ApiKey. Если нет - прописываю свой. Временно. По факту у каждого должен быть свой ключ.
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string ApiKey = ConfigurationManager.AppSettings.Get("ApiKey");

            if (ApiKey != string.Empty && ApiKey != null && ApiKey != "0")
            {
                return ApiKey;
            }
            else
            {
                config.AppSettings.Settings["ApiKey"].Value = "9TRb7MdTltQA0AnYGirnmTuqrusK1qdM";
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                ApiKey = ConfigurationManager.AppSettings.Get("ApiKey");

                return ApiKey; 
            }
        }
        public int GetLocationKey() // Получаем код города\региона, исходя из нашего IP
        {
            string locationKey = ConfigurationManager.AppSettings.Get("LocationKey");

            if (locationKey != null && locationKey != string.Empty && locationKey != "0")
            {
                return Convert.ToInt32(locationKey);
            }
            else
            { 
                using (var client = new RestClient(SearchCity()))
                {
                    try
                    {
                        var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        var request = new RestRequest();
                        var response = client.PostAsync(request).Result.Content;
                        var result = GetCityFromJson(response);

                        config.AppSettings.Settings["LocationKey"].Value = result.Key;
                        config.Save();
                        ConfigurationManager.RefreshSection("appSettings");

                        return Convert.ToInt32(result.Key);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Возникла проблема, при получении данных:\n {e.Message}");
                        return 404;
                    }
                }
            }
        }
    }
}
