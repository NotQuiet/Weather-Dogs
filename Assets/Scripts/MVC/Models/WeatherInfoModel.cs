using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using Custom;
using Cysharp.Threading.Tasks;
using MVC.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace MVC.Models
{
    public class WeatherInfoModel : Model
    {
        [Inject] private WebRequestService _webRequestService;

        private string URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        private ConcurrentQueue<string> _requests = new();

        public ReactiveCommand<string> Temperature = new();

        public void GetWeather()
        {
            GetTemperature();
        }

        public void CancelRequests()
        {
            foreach (var _ in _requests)
            {
                _requests.TryDequeue(out string id);
                _webRequestService.CancelRequest(id);
            }
        }

        private void GetTemperature()
        {
            var requestId = _webRequestService.EnqueueRequest(URL, OnSuccess, OnError);
            _requests.Enqueue(requestId);
        }

        private void OnSuccess(WebRequestDto responseJson)
        {
            _requests.TryDequeue(out _);

            try
            {
                WeatherResponse weatherData = JsonUtility.FromJson<WeatherResponse>(responseJson.Response);
                if (weatherData != null && weatherData.properties.periods.Length > 0)
                {
                    Temperature.Execute(weatherData.properties.periods[0].temperature
                        .ToString(CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка парсинга JSON: {ex.Message}");
            }
        }

        private void OnError(WebRequestDto response)
        {
            _requests.TryDequeue(out _);

            Debug.LogError($"Error get temperature: {response.Response}");
        }
    }

    [Serializable]
    public class WeatherResponse
    {
        public Properties properties;
    }

    [Serializable]
    public class Properties
    {
        public Period[] periods;
    }

    [Serializable]
    public class Period
    {
        public float temperature;
    }
}