using Custom;
using MVC.Abstract;
using MVC.Models;
using MVC.Views;
using UniRx;

namespace MVC.Controllers
{
    public class WeatherInfoController : Controller<WeatherInfoModel, WeatherInfoView>
    {
        public WeatherInfoController(WeatherInfoModel model, WeatherInfoView view) : base(model, view)
        {
        }

        public CompositeDisposable Disposable { get; } = new();
        public ReactiveCommand<string> SetWeather { get; } = new();


        public override void Init(EventBus bus)
        {
            base.Init(bus);

            bus.OnWeatherClicked.Subscribe(_ => { OnWeatherButtonClicked(); }).AddTo(Disposable);
            bus.OnDogsClicked.Subscribe(_ => { OnDogsButtonClicked(); }).AddTo(Disposable);

            Model.Temperature.Subscribe(s => { SetWeather.Execute(s); }).AddTo(Disposable);
        }

        private void OnWeatherButtonClicked()
        {
            Model.GetWeather();
        }

        private void OnDogsButtonClicked()
        {
        }
    }
}