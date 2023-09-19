using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Resto.Front.Api.AccuWeather.ConfigAWeather
{
    public class Imperial
    {
        [JsonProperty("Value")]
        public double? Value { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }
    }
    public class Temperature
    {
        [JsonProperty("Metric")]
        public Imperial Metric { get; set; }

        [JsonProperty("Imperial")]
        public Imperial Imperial { get; set; }

        [JsonProperty("Value")]
        public double? Value { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }
    }
    public class AWeatherVariables : IComparable<AWeatherVariables>
    {
        [JsonProperty("DateTime")]
        public DateTimeOffset? DateTime { get; set; }

        [JsonProperty("WeatherIcon")]
        public int? WeatherIcon { get; set; }

        [JsonProperty("IconPhrase")]
        public string IconPhrase { get; set; }

        [JsonProperty("Temperature")]
        public Temperature Temperature { get; set; }

        [JsonProperty("PrecipitationProbability")]
        public int? PrecipitationProbability { get; set; }

        [JsonProperty("RainProbability")]
        public int? RainProbability { get; set; }

        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("LocalizedName")]
        public string LocalizedName { get; set; }

        [JsonProperty("LocalObservationDateTime")]
        public DateTimeOffset? LocalObservationDateTime { get; set; } // OR set Type DateTime

        [JsonProperty("WeatherText")]
        public string WeatherText { get; set; }

        public int CompareTo(AWeatherVariables temp)
        {
            return (int)Temperature.Value;
        }
    }
}
