using MVC.Models;
using TMPro;
using UnityEngine;

namespace Custom
{
    public class DogItemCell : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI number;
        [SerializeField] private TextMeshProUGUI name;

        private DogItemDto _thisDog;

        public void Init(DogItemDto thisDog)
        {
            _thisDog = thisDog;
            
            SetInfo();
        }

        private void SetInfo()
        {
            name.text = _thisDog.name;
            number.text = _thisDog.id.ToString();
        }
    }
}