using Custom;
using MVC.Abstract;
using MVC.Controllers;
using MVC.Models;
using UniRx;
using UnityEngine;

namespace MVC.Views
{
    public class ButtonsView : View<ButtonsModel, ButtonsView, ButtonsController>
    {
        [SerializeField] private CustomButton weatherButton;
        [SerializeField] private CustomButton dogsButton;

        public override void OnInitialized()
        {

        }
    }
}