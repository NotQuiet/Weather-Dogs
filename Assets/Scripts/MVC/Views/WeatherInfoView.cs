using DG.Tweening;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using TMPro;
using UniRx;
using UnityEngine;

namespace MVC.Views
{
    public class WeatherInfoView : View<WeatherInfoModel, WeatherInfoView, WeatherInfoController>
    {
        [SerializeField] private TextMeshProUGUI weatherInfoText;
        
        public override void OnInitialized()
        {
            base.OnInitialized();

            Controller.SetWeather.Subscribe(s =>
            {
                DOTween.Sequence()
                    .Append(transform.DOScale(1.2f, 0.2f))
                    .Append(transform.DOScale(1f, 0.1f));
                
                weatherInfoText.text = $"Today {s}°F";
            }).AddTo(Controller.Disposable);
        }
    }
}