using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public ReactiveCommand OnClick { get; } = new();

        public void OnPointerDown(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Append(transform.DOScale(1.1f, 0.1f))
                .Append(transform.DOScale(1f, 0.1f));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnClick.Execute();
        }
    }
}