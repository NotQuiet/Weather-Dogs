using Custom;
using MVC.Abstract;
using MVC.Models;
using MVC.Views;
using UniRx;

namespace MVC.Controllers
{
    public class DogsController : Controller<DogsModel, DogsView>
    {
        public DogsController(DogsModel model, DogsView view) : base(model, view)
        {
        }

        public CompositeDisposable Disposable { get; } = new();
        
        public ReactiveCommand TurnOnDogs = new();
        public ReactiveCommand TurnOffDogs = new();

        public override void Init(EventBus bus)
        {
            base.Init(bus);

            bus.OnWeatherClicked.Subscribe(_ => { OnWeatherButtonClicked(); }).AddTo(Disposable);
            bus.OnDogsClicked.Subscribe(_ => { OnDogsButtonClicked(); }).AddTo(Disposable);
        }

        private void OnWeatherButtonClicked()
        {
            TurnOffDogs.Execute();
        }

        private void OnDogsButtonClicked()
        {
            TurnOnDogs.Execute();
        }
    }
}