using Cysharp.Threading.Tasks;
using DG.Tweening;
using MVC.Models;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Custom
{
    public class DogItemCell : MonoBehaviour
    {
        [SerializeField] private CustomButton customButton;
        [SerializeField] private TextMeshProUGUI number;
        [SerializeField] private TextMeshProUGUI dogName;
        [SerializeField] private Image loadingImage;
        
        private CompositeDisposable _disposable = new();
        private DogItemDto _thisDog;
        
        private bool _isLoading;
        
        public DogItemDto GetDog() => _thisDog;
        
        public ReactiveCommand<DogItemDto> NeedToShowDog { get; } = new();

        public void Init(DogItemDto thisDog, int num)
        {
            _thisDog = thisDog;
            _disposable.Clear();

            SetInfo(num);

            customButton.OnClick.Subscribe(_ =>
            {
                OnCellClick();
            }).AddTo(_disposable);
        }

        private void SetInfo(int num)
        {
            dogName.text = _thisDog.name;
            number.text = num.ToString();
        }
        
        private void OnCellClick()
        {
            NeedToShowDog.Execute(_thisDog);
        }

        public void RotateLoading()
        {
            loadingImage.DOFade(1f, 0.1f);
            _isLoading = true;
            Loading();
        }

        public void StopRotateLoading()
        {
            loadingImage.DOFade(0f, 0.1f);
            _isLoading = false;
        }
        
        private async void Loading()
        {
            float rotationSpeed = -180f;

            while (_isLoading)
            {
                loadingImage.transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
                await UniTask.Yield();
            }
        }
    }
}