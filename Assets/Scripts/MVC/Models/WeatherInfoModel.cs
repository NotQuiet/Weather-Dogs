using System;
using Cysharp.Threading.Tasks;
using MVC.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace MVC.Models
{
    public class WeatherInfoModel : Model
    {
        private string URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
            
        public ReactiveCommand<string> Temperature = new();
        
        public async void GetWeather()
        {
            float? temperature = await GetTemperature();

            Temperature.Execute(temperature.ToString());
        }
        
        private async UniTask<float?> GetTemperature()
        {
            using UnityWebRequest request = UnityWebRequest.Get(URL);
            request.SetRequestHeader("User-Agent", "MyUnityApp/1.0 (myemail@example.com)");

            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                try
                {
                    WeatherResponse weatherData = JsonUtility.FromJson<WeatherResponse>(json);
                    if (weatherData != null && weatherData.properties.periods.Length > 0)
                    {
                        return weatherData.properties.periods[0].temperature;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка парсинга JSON: {ex.Message}");
                }
            }
            else
            {
                Debug.LogError("Ошибка запроса: " + request.error);
            }

            return null;
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