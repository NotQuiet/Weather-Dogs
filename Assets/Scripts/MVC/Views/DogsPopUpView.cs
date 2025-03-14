using Custom;
using DG.Tweening;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using TMPro;
using UniRx;
using UnityEngine;

namespace MVC.Views
{
    public class DogsPopUpView : View<DogsPopUpModel, DogsPopUpView, DogsPopUpController>
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private CustomButton okButton;
        [SerializeField] private CanvasGroup canvasGroup;
        
        
        public override void OnInitialized()
        {
            base.OnInitialized();
            
            Controller.SetDogDescription.Subscribe(SetDogDescription).AddTo(Controller.Disposable);

            okButton.OnClick.Subscribe(_ =>
            {
                HidePopUp();
            }).AddTo(Controller.Disposable);
        }

        private void SetDogDescription((string title, string description) valueTuple)
        {
            titleText.text = valueTuple.title;
            descriptionText.text = valueTuple.description;

            EventBus.OnDogDescriptionLoaded.Execute(valueTuple.title);
            
            ShowPopUp();
        }

        private void ShowPopUp()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.DOFade(1f, 0.2f);
        }

        private void HidePopUp()
        {
            canvasGroup.DOFade(0f, 0.2f);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}