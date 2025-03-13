using Zenject;

namespace MVC.Abstract
{
    public class View<TM, TV, TC> : ViewBase where TM : Model where TV : ViewBase where TC : Controller<TM, TV>
    {
        [Inject] protected TC Controller;
    }
}