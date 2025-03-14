using MVC.Models;
using UniRx;
using Zenject;

namespace Custom
{
    public class EventBus
    {
        public ReactiveCommand OnWeatherClicked { get; } = new();
        public ReactiveCommand OnDogsClicked { get; } = new();
        public ReactiveCommand<DogItemDto> ShowDog { get; } = new();
        public ReactiveCommand OnStartLoading { get; } = new();
        public ReactiveCommand OnEndLoading { get; } = new();
        public ReactiveCommand<string> OnDogDescriptionLoaded { get; } = new();
    }
}