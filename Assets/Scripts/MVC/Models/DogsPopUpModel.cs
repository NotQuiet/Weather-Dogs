using System;
using Cysharp.Threading.Tasks;
using MVC.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace MVC.Models
{
    public class DogsPopUpModel : Model
    {
        private const string BASE_URL = "https://dogapi.dog/api/v2/breeds";

        public ReactiveCommand<string> DogDescription = new();
        
        public async void GetDog(string id)
        {
            string description = await GetBreedDescription(id);
            Debug.Log("On get dog Description: " + description);
            DogDescription.Execute(description);
        }

        private async UniTask<string> GetBreedDescription(string id)
        {
            string url = $"{BASE_URL}/{id}";

            using UnityWebRequest request = UnityWebRequest.Get(url);
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log(json); // Выводим JSON для проверки

                try
                {
                    DogDescriptionResponse response = JsonUtility.FromJson<DogDescriptionResponse>(json);
                    return response.data.attributes.description;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка парсинга JSON: {ex.Message}\nJSON: {json}");
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
    public class DogDescriptionResponse
    {
        public DogData data;
    }

    [Serializable]
    public class DogData
    {
        public DogDescriptionAttributes attributes;
    }

    [Serializable]
    public class DogDescriptionAttributes
    {
        public string description;
    }
}