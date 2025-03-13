using System.Collections.Generic;
using Custom;
using DG.Tweening;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using UniRx;
using UnityEngine;

namespace MVC.Views
{
    public class DogsItemsView : View<DogsItemsModel, DogsItemsView, DogsItemsController>
    {
        [SerializeField] private DogItemCell dogItemCellPrefab;
        [SerializeField] private Transform content;

        private void SetContent(List<DogItemDto> dogs)
        {
            foreach (var dog in dogs)
            {
                var dogObj = Instantiate(dogItemCellPrefab, content);
                dogObj.Init(dog);
            }
        }
        
        public override void OnInitialized()
        {
            base.OnInitialized();

            Controller.SetDogs.Subscribe(SetContent).AddTo(Controller.Disposable);
        }
    }
}