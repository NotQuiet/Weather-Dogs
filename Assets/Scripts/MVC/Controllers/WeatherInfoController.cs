using System.Threading;
using Custom;
using Cysharp.Threading.Tasks;
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

        private CancellationTokenSource _cancellationTokenSource;

        public CompositeDisposable Disposable { get; } = new();
        public ReactiveCommand<string> SetWeather { get; } = new();


        private bool _loopIsActive;

        public override void Init(EventBus bus)
        {
            base.Init(bus);

            StartGettingWeather();

            bus.OnWeatherClicked.Subscribe(_ => { StartGettingWeather(); }).AddTo(Disposable);
            bus.OnDogsClicked.Subscribe(_ => { CancelLoop(); }).AddTo(Disposable);

            Model.Temperature.Subscribe(s => { SetWeather.Execute(s); }).AddTo(Disposable);
        }

        private void StartGettingWeather()
        {
            if (_loopIsActive)
                return;
            
            _cancellationTokenSource = new CancellationTokenSource();

            GetWeatherLoop();
        }

        private async void GetWeatherLoop()
        {
            _loopIsActive = true;

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                Model.GetWeather();
                await UniTask.Delay(5000);
            }

            _loopIsActive = false;
        }

        private void CancelLoop()
        {
            Model.CancelRequests();
            _loopIsActive = false;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}