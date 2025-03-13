using Custom;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using UniRx;
using UnityEngine;

namespace MVC.Views
{
    public class ButtonsView : View<ButtonsModel, ButtonsView, ButtonsController>
    {
        [SerializeField] private CustomButton weatherButton;
        [SerializeField] private CustomButton dogsButton;

        public override void OnInitialized()
        {
            Controller.Init(EventBus);

            weatherButton.OnClick.Subscribe(_ => { EventBus.OnWeatherClicked.Execute(); }).AddTo(Controller.Disposable);

            dogsButton.OnClick.Subscribe(_ => { EventBus.OnDogsClicked.Execute(); }).AddTo(Controller.Disposable);
        }
    }
}