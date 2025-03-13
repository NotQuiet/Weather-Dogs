using MVC.Abstract;
using MVC.Models;
using MVC.Views;

namespace MVC.Controllers
{
    public class ButtonsController : Controller<ButtonsModel, ButtonsView>
    {
        public ButtonsController(ButtonsModel model, ButtonsView view) : base(model, view)
        {
        }
    }
}