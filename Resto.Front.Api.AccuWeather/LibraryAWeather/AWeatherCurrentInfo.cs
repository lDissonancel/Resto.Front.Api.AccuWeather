using Resto.Front.Api.AccuWeather.ConfigAWeather;
using System;

namespace Resto.Front.Api.AccuWeather.LibraryAWeather
{
    public class AWeatherCurrentInfo : AWeatherVariables
    {
        public virtual string GetCurrentWeatherText()
        {
            return $"На улице: {WeatherText}";
        }
        public virtual string GetCurrentLocalObservationDateTime()
        {
            return $"Дата и время: {LocalObservationDateTime:g}";
        }
        public virtual string GetCurrentTemperature()
        {
            return $"Текущая температруа: {Math.Round(Temperature.Metric.Value ?? 0)} {Temperature.Metric.Unit}";
        }
        public virtual string GetCurrentInfoWeather()
        {
            return $"{GetCurrentWeatherText()}\n{GetCurrentTemperature()}\n{GetCurrentLocalObservationDateTime()}\n";
        }
    }
}
