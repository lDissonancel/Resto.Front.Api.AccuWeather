using Newtonsoft.Json;
using Resto.Front.Api.AccuWeather.ConfigAWeather;
using Resto.Front.Api.AccuWeather.LibraryAWeather;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Front.Api.AccuWeather.ReqAWeather
{
    public class RequestAWeather
    {
        private readonly AWeatherConfig configWeather = new AWeatherConfig();
        private static AWeatherCurrentInfo[] GetCurrentWeatherFromJson(string json) => JsonConvert.DeserializeObject<AWeatherCurrentInfo[]>(json);
        private static List<AWeatherForecastInfo> GetForecastWeatheFromJson(string json) => JsonConvert.DeserializeObject<List<AWeatherForecastInfo>>(json);

        public string GetCurrentWeather()
        {
            using (var client = new RestClient($"{configWeather.GetCurrentConditions()}"))
            {
                try
                {
                    var request = new RestRequest();
                    var response = client.PostAsync(request).Result.Content;
                    var result = GetCurrentWeatherFromJson(response);

                    return result[0].GetCurrentInfoWeather();
                }
                catch (Exception e)
                {
                    return $"Возникла проблема, при получении данных:\n{e.Message}";
                }
            }
        }

        public string GetRecomendedFor12Hours()
        {
            using (var client = new RestClient($"{configWeather.GetWeatherFor12Hours()}"))
            {
                try
                {
                    var request = new RestRequest();
                    var response = client.PostAsync(request).Result.Content;
                    var result = GetForecastWeatheFromJson(response);

                    AWeatherForecastInfo getRecomended = new AWeatherForecastInfo();

                    return $"Прогноз на ближайшие 12 часов:\n" +
                        $"{getRecomended.GetRecomendedForWeatherToDay(result)}\n" +
                        $"{getRecomended.GetRecomendedForTemperatureToDay(result)}";
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    return $"Возникла проблема, при получении данных:\n {e.Message}";
                }
            }
        }

        public string TwelveHoursOfHourlyForecasts()
        {
            using (var client = new RestClient($"{configWeather.GetWeatherFor12Hours()}"))
            {
                try
                {
                    var request = new RestRequest();
                    var response = client.PostAsync(request).Result.Content;
                    var result = GetForecastWeatheFromJson(response);
                    string recomended = string.Empty;

                    return string.Join("----------------------------\n", result.Select(c => c.GetInfoForecastWeather()));
                }
                catch (Exception e)
                {
                    return $"Возникла проблема, при получении данных:\n{e.Message}";
                }
            }
        }
    }
}
