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

        public override void OnInitialized()
        {
            base.OnInitialized();
            SetPoolSize(10);

            Controller.SetDogs.Subscribe(SetContent).AddTo(Controller.Disposable);

            EventBus.OnDogDescriptionLoaded.Subscribe(OnDogDescriptionLoaded).AddTo(Controller.Disposable);
        }

        private void OnDogDescriptionLoaded(string title)
        {
            foreach (var dogCell in _pool.PoolArray)
            {
                if (dogCell.GetDog().name != title) continue;
                
                dogCell.StopRotateLoading();
                break;
            }
        }

        private void SetContent(List<DogItemDto> dogs)
        {
            int num = 0;
            foreach (var dog in dogs)
            {
                num++;

                var dogObj = _pool.GetItem(dogCellFactory, content);

                dogObj.Init(dog, num);

                dogObj.NeedToShowDog.Subscribe(dto => { OnCellClicked(dto, dogObj); }).AddTo(Controller.Disposable);
            }
        }

        private void OnCellClicked(DogItemDto dogDto, DogItemCell cell)
        {
            foreach (var dogCell in _pool.PoolArray)
            {
                dogCell.StopRotateLoading();
            }

            cell.RotateLoading();

            EventBus.ShowDog.Execute(dogDto);
        }


        private void SetPoolSize(int size)
        {
            _pool.SetPoolSize(size);
        }
    }
}