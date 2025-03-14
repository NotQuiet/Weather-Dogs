using Custom;
using MVC.Abstract;
using MVC.Models;
using MVC.Views;
using UniRx;

namespace MVC.Controllers
{
    public class LoadingController : Controller<LoadingModel, LoadingView>
    {
        public LoadingController(LoadingModel model, LoadingView view) : base(model, view)
        {
        }

        public override void Init(EventBus bus)
        {
            base.Init(bus);
        }
    }
}