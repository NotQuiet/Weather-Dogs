using Custom;
using UniRx;
using UnityEngine;

namespace MVC.Abstract
{
    public class Controller <TM, TV> where TM : Model where TV : ViewBase
    {
        public TM Model;
        public TV View;

        private EventBus _eventBus;
        public CompositeDisposable Disposable { get; } = new();

        public virtual void Init(EventBus bus)
        {
            _eventBus = bus;
        }

        public Controller(TM model, TV view)
        {
            Model = model;
            View = view;
            Debug.Log($"Controller<{typeof(TM).Name}, {typeof(TV).Name}> initialized");
        }
    }

}