using Cysharp.Threading.Tasks;
using DG.Tweening;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.Views
{
    public class LoadingView : View<LoadingModel, LoadingView, LoadingController>
    {
        [SerializeField] private Image loadingImage;
        
        private bool _isLoading;
        
        public override void OnInitialized()
        {
            base.OnInitialized();

            EventBus.OnStartLoading.Subscribe(_ =>
            {
                StartLoading();
            }).AddTo(Controller.Disposable);
            
            EventBus.OnEndLoading.Subscribe(_ =>
            {
                StopLoading();
            }).AddTo(Controller.Disposable);
        }

        private void StartLoading()
        {
            _isLoading = true;
            loadingImage.DOFade(1f, 0.1f);
            Loading();
        }

        private void StopLoading()
        {
            loadingImage.DOFade(0f, 0.1f);
            _isLoading = false;
        }

        private async void Loading()
        {
            while (_isLoading)
            {
                float rotationSpeed = -180f;
                loadingImage.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
                await UniTask.Yield();
            }
        }
    }
}