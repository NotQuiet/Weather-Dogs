using MVC.Models;
using TMPro;
using UniRx;
using UnityEngine;

namespace Custom
{
    public class DogItemCell : MonoBehaviour
    {
        [SerializeField] private CustomButton customButton;
        [SerializeField] private TextMeshProUGUI number;
        [SerializeField] private TextMeshProUGUI dogName;
        
        private CompositeDisposable _disposable = new();
        private DogItemDto _thisDog;
        
        public ReactiveCommand<DogItemDto> NeedToShowDog { get; } = new();

        public void Init(DogItemDto thisDog, int num)
        {
            _thisDog = thisDog;
            _disposable.Clear();

            SetInfo(num);

            customButton.OnClick.Subscribe(_ =>
            {
                NeedToShowDog.Execute(_thisDog);
            }).AddTo(_disposable);
        }

        private void SetInfo(int num)
        {
            dogName.text = _thisDog.name;
            number.text = num.ToString();
        }
    }
}