using UnityEngine;

namespace MVC.Abstract
{
    public class Controller <TM, TV> where TM : Model where TV : ViewBase
    {
        public TM Model;
        public TV View;

        public Controller(TM model, TV view)
        {
            Model = model;
            View = view;
            Debug.Log($"Controller<{typeof(TM).Name}, {typeof(TV).Name}> initialized");
        }
    }

}