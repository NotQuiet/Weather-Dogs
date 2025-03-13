using DG.Tweening;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using UniRx;
using UnityEngine;

namespace MVC.Views
{
    public class WeatherView : View<WeatherModel, WeatherView, WeatherController>
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        public override void OnInitialized()
        {
            base.OnInitialized();
            
            Controller.TurnOffWeather.Subscribe(_ =>
            {
                canvasGroup.DOFade(0, 0.1f);
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }).AddTo(Controller.Disposable);
            
            Controller.TurnOnWeather.Subscribe(_ =>
            {
                canvasGroup.DOFade(1, 0.1f);
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }).AddTo(Controller.Disposable);
        }
    }
}