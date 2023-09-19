using Resto.Front.Api.AccuWeather.ReqAWeather;
using System;
using System.Reactive.Disposables;

namespace Resto.Front.Api.AccuWeather
{
    using static PluginContext;
    internal sealed class ButtonOkPopUp : IDisposable
    {
        private readonly CompositeDisposable subscriptions;
        private readonly RequestAWeather requestAWeather = new RequestAWeather();
        
        public ButtonOkPopUp()
        {
            subscriptions = new CompositeDisposable()
            {
                Operations.AddButtonToPluginsMenu("Погода: Текущая", x=> x.vm.ShowOkPopup("Погода", $"{requestAWeather.GetCurrentWeather()}")),
                Operations.AddButtonToPluginsMenu("Погода: На ближайшие 12 часов", x=> x.vm.ShowOkPopup("Погода", $"{requestAWeather.GetRecomendedFor12Hours()}"))
            };
        }
        public void Dispose()
        {
            subscriptions.Dispose();
        }
    }
}
