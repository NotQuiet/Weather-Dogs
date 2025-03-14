using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
        public ReactiveCommand OnClick { get; } = new();

        public void OnPointerDown(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Append(transform.DOScale(1.1f, 0.1f))
                .Append(transform.DOScale(1f, 0.1f));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Execute();
        }
    }
}