using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Custom
{
    public class CustomButton : MonoBehaviour, IPointerUpHandler
    {
        public ReactiveCommand OnClick { get; } = new();
        
        public void OnPointerUp(PointerEventData eventData)
        {
            OnClick.Execute();
        }
    }
}