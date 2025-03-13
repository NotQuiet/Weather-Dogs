using System.Collections.Generic;
using Custom;
using MVC.Abstract;
using MVC.Models;
using MVC.Views;
using UniRx;

namespace MVC.Controllers
{
    public class DogsItemsController : Controller<DogsItemsModel, DogsItemsView>
    {
        public DogsItemsController(DogsItemsModel model, DogsItemsView view) : base(model, view)
        {
        }

        public CompositeDisposable Disposable { get; } = new();
        public ReactiveCommand<List<DogItemDto>> SetDogs { get; } = new();


        public override void Init(EventBus bus)
        {
            base.Init(bus);

            bus.OnDogsClicked.Subscribe(_ => { OnDogsButtonClicked(); }).AddTo(Disposable);

            Model.DogList.Subscribe(s => { SetDogs.Execute(s); }).AddTo(Disposable);
        }

        private void OnDogsButtonClicked()
        {
            Model.GetDogs();
        }
    }
}