using Zenject;

namespace MVC.Abstract
{
    public class View<TM, TV> : ViewBase where TM : Model where TV : ViewBase
    {
        [Inject] protected Controller<TM, TV> Controller;
    }
}