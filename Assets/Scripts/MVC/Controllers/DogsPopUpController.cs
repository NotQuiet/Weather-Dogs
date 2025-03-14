using System.Collections.Generic;
using Custom;
using MVC.Abstract;
using MVC.Models;
using MVC.Views;
using UniRx;
using UnityEngine;

namespace MVC.Controllers
{
    public class DogsPopUpController : Controller<DogsPopUpModel, DogsPopUpView>
    {
        public DogsPopUpController(DogsPopUpModel model, DogsPopUpView view) : base(model, view)
        {
        }

        public ReactiveCommand<(string title, string description)> SetDogDescription { get; } = new();

        private string _cachedDogTitle;
        
        public override void Init(EventBus bus)
        {
            base.Init(bus);

            bus.ShowDog.Subscribe(GetDogDescription).AddTo(Disposable);

            Model.DogDescription.Subscribe(d =>
            {
                SetDogDescription.Execute((_cachedDogTitle, d));
            }).AddTo(Disposable);
        }

        private void GetDogDescription(DogItemDto dto)
        {
            _cachedDogTitle = dto.name;
            
            Model.CancelLastRequest();
            Model.GetDog(dto.id);
        }
    }
}