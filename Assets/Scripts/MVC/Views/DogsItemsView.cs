using System.Collections.Generic;
using System.ComponentModel;
using Custom;
using Custom.Factories;
using Custom.Pools;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using UniRx;
using UnityEngine;

namespace MVC.Views
{
    public class DogsItemsView : View<DogsItemsModel, DogsItemsView, DogsItemsController>
    {
        [SerializeField] protected GenericFactory<DogItemCell> dogCellFactory;
        [SerializeField] private Transform content;

        private AbstractPool<DogItemCell> _pool = new();

        private void SetContent(List<DogItemDto> dogs)
        {
            int num = 0;
            foreach (var dog in dogs)
            {
                num++;

                var dogObj = _pool.GetItem(dogCellFactory, content);

                dogObj.Init(dog, num);

                dogObj.NeedToShowDog.Subscribe(dto =>
                {
                    Debug.Log($"Show dog {dto.name}");

                    EventBus.ShowDog.Execute(dto);
                }).AddTo(Controller.Disposable);
            }
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            SetPoolSize(10);

            Controller.SetDogs.Subscribe(SetContent).AddTo(Controller.Disposable);
        }

        private void SetPoolSize(int size)
        {
            _pool.SetPoolSize(size);
        }
    }
}