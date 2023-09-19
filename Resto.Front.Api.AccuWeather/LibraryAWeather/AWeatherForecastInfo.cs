using Resto.Front.Api.AccuWeather.ConfigAWeather;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Front.Api.AccuWeather.LibraryAWeather
{
    public class AWeatherForecastInfo : AWeatherVariables
    {
        public string GetForecastTemperature() // Температура с указанием метрики (Цельсия)
        {
            return $"Прогнозируемая температура: {Math.Round(Temperature.Value ?? 0)} {Temperature.Unit}";
        }

        public string GetForecastIconPhrase() // Какая погода на улице (ясно, солнчено и т.д.) за указанный период
        {
            return $"На улице ожидается: {IconPhrase}";
        }

        public string GetForecastDateTime() // Дата и время
        {
            return $"Дата и время: {DateTime:g}";
        }

        public string GetInfoForecastWeather() // Общая информация по погоде за указанный период
        {
            return $"{GetForecastIconPhrase()}\n{GetForecastTemperature()}\n{GetForecastDateTime()}\n";
        }

        public int GetMaxTemperature(List<AWeatherForecastInfo> temperatureList) // Максимальная температра за указанный период
        {
            return (int)Math.Round(temperatureList.Max().Temperature.Value ?? 0);
        }

        public int GetMinTemperature(List<AWeatherForecastInfo> temperatureList) // Минимальная температра за указанный период
        {
            return (int)Math.Round(temperatureList.Min().Temperature.Value ?? 0);
        }

        public int GetAverageTemperature(List<AWeatherForecastInfo> temperatureList) // Средняя температра за указанный период
        {
            return (int)Math.Round(temperatureList.Average(c => c.Temperature.Value?? 0));
        }

        public int GetRainProbabilityPercent(List<AWeatherForecastInfo> temperatureList) // Вероятность дождя за указанный период
        {
            return (int)Math.Round(temperatureList.Average(c => c.RainProbability ?? 0));
        }

        public int GetPrecipitationProbabilityPercent(List<AWeatherForecastInfo> temperatureList) // Вероятность осадков за указанный период
        {
            if (temperatureList == null || temperatureList.Count() == 0)
            {
                return 0;
            }

            return temperatureList.Sum(c => c.PrecipitationProbability ?? 0) / temperatureList.Count();
        }

        public string GetMinWeatherIconPhrase(List<AWeatherForecastInfo> weatherList) // Погода, которая будет стоять на улице наименьшее кол-во времени за указанный период
        {
            int minWeatherIcon = int.MaxValue;
            string minIconPhrase = string.Empty;

            if (weatherList == null || weatherList.Count() == 0)
            {
                return "Нет данных о погоде";
            }
            else
            {
                foreach (var item in weatherList)
                {
                    if (weatherList.Count(c => c.WeatherIcon.Value == item.WeatherIcon.Value) < minWeatherIcon)
                    {
                        minWeatherIcon = Convert.ToInt32(weatherList.IndexOf(item));
                    }
                    if (item == weatherList[weatherList.Count() - 1])
                    {
                        minIconPhrase = weatherList[minWeatherIcon].IconPhrase;
                        break;
                    }
                }
            }

            return minIconPhrase;
        }

        public string GetMaxWeatherIconPhrase(List<AWeatherForecastInfo> weatherList) // Погода, которая будет стоять на улице наибольшее кол-во времени за указанный период
        {
            int maxWeatherIcon = int.MinValue;
            string maxIconPhrase = string.Empty;

            if (weatherList == null || weatherList.Count() == 0)
            {
                return "Нет данных о погоде";
            }
            else
            {
                foreach (var item in weatherList)
                {
                    if (weatherList.Count(c => c.WeatherIcon.Value == item.WeatherIcon.Value) > maxWeatherIcon)
                    {
                        maxWeatherIcon = Convert.ToInt32(weatherList.IndexOf(item));
                    }
                    if (item == weatherList[weatherList.Count() - 1])
                    {
                        maxIconPhrase = weatherList[maxWeatherIcon].IconPhrase;
                        break;
                    }
                }
            }

            return maxIconPhrase;
        }

        public string GetRecomendedForTemperatureToDay(List<AWeatherForecastInfo> temperatureList) // Макс. Мин. и Ср. Температура за 12 часов
        {
            int maxTemperature = GetMaxTemperature(temperatureList);
            int minTemperature = GetMinTemperature(temperatureList);
            int averageTemperature = GetAverageTemperature(temperatureList);

            int percentRainToday = GetRainProbabilityPercent(temperatureList);
            int percentPrecipitationToday = GetPrecipitationProbabilityPercent(temperatureList);

            string recomended =
                averageTemperature > 18 && averageTemperature <= 25
                ? "Сегодня тепло. Можно насладиться прогулкой после работы."
                : averageTemperature <= 18 && averageTemperature > 12
                ? "Сегодня прохладно. Лучше иметь при себе куртку."
                : averageTemperature <= 12
                ? "Сегодня холодно. Одевайся потеплее."
                : "Сегодня жарко. Можно оставить куртку дома.";

            recomended += percentRainToday >= 40 || percentPrecipitationToday >= 40
                ? " Однако сегодня высокая вроятность осадков или дождя. Лучше иметь при себе зонтик."
                : percentRainToday != 0 && percentPrecipitationToday != 0
                ? " Также есть небольшая вероятность осадков или дождя."
                : " Дождей или осадков не ожидается.";


            return $"Температура от {minTemperature} {temperatureList[0].Temperature.Unit} " +
                   $"до {maxTemperature} {temperatureList[0].Temperature.Unit}.\n" +
                   $"Средняя температура {averageTemperature}.\n" +
                   $"{recomended} :)";
        }

        public string GetRecomendedForWeatherToDay(List<AWeatherForecastInfo> weatherList) // Показатель основной погоды за 12 часов (исходя из того, какое значение WeatherIcon преобладает)
        {
            string maxIconPhrase = GetMaxWeatherIconPhrase(weatherList);
            string minIconPhrase = GetMinWeatherIconPhrase(weatherList);

            return minIconPhrase == maxIconPhrase || minIconPhrase.ToLower().Contains(maxIconPhrase.ToLower())
                   ? $"Весь день {maxIconPhrase.ToLower()}"
                   : $"{maxIconPhrase}{minIconPhrase.ToLower()}.";
        }
    }
}
