using UnityEngine;
using Zenject;

namespace MVC.Abstract
{
    public class ViewBase : MonoBehaviour
    {
        [Inject]
        public virtual void OnInitialized() { } 
    }
}