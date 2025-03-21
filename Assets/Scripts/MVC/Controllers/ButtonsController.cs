using Custom;
using MVC.Abstract;
using MVC.Models;
using MVC.Views;
using UniRx;
using UnityEngine;

namespace MVC.Controllers
{
    public class ButtonsController : Controller<ButtonsModel, ButtonsView>
    {
        public ButtonsController(ButtonsModel model, ButtonsView view) : base(model, view)
        {
        }


        public override void Init(EventBus bus)
        {
            base.Init(bus);
        }
    }
}