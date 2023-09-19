using System;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;

namespace Resto.Front.Api.AccuWeather
{
    [UsedImplicitly]
    [PluginLicenseModuleId(21005108)]
    public sealed class WeatherPlugin : IFrontPlugin
    {
        private readonly Stack<IDisposable> subscriptions = new Stack<IDisposable>();
        public WeatherPlugin() 
        {
            PluginContext.Log.Info("Плагин запускается...");
            subscriptions.Push(new ButtonOkPopUp());
            PluginContext.Log.Info("Плагин успешно запустился!");
        }
        public void Dispose()
        {
            while (subscriptions.Any())
            {
                var subscription = subscriptions.Pop();
                try
                {
                    subscription.Dispose();
                }
                catch (RemotingException e)
                {
                    PluginContext.Log.Error(e.Message.ToString());
                }
            }

            PluginContext.Log.Info("Плагин завершил работу...");
        }
    }
}
