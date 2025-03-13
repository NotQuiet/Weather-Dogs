using UniRx;
using Zenject;

namespace Custom
{
    public class EventBus
    {
        public ReactiveCommand OnWeatherClicked { get; } = new();
        public ReactiveCommand OnDogsClicked { get; } = new();
    }
}