using Custom;
using MVC.Abstract;
using MVC.Models;
using MVC.Views;
using UniRx;
using UnityEngine;

namespace MVC.Controllers
{
    public class WeatherController : Controller<WeatherModel, WeatherView>
    {
        public WeatherController(WeatherModel model, WeatherView view) : base(model, view)
        {
        }

        public ReactiveCommand TurnOnWeather = new();
        public ReactiveCommand TurnOffWeather = new();

        public override void Init(EventBus bus)
        {
            base.Init(bus);

            bus.OnWeatherClicked.Subscribe(_ => { OnWeatherButtonClicked(); }).AddTo(Disposable);
            bus.OnDogsClicked.Subscribe(_ => { OnDogsButtonClicked(); }).AddTo(Disposable);
        }

        private void OnWeatherButtonClicked()
        {
            TurnOnWeather.Execute();
        }

        private void OnDogsButtonClicked()
        {
            TurnOffWeather.Execute();
        }
    }
}