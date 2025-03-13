using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MVC.Abstract;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace MVC.Models
{
    public class DogsItemsModel : Model
    {
        private const string URL = "https://api.thedogapi.com/v1/breeds";

        public ReactiveCommand<List<DogItemDto>> DogList = new();

        public async void GetDogs()
        {
            var dogs = await GetBreeds();

            DogList.Execute(dogs);
        }

        private async UniTask<List<DogItemDto>> GetBreeds()
        {
            using UnityWebRequest request = UnityWebRequest.Get(URL);
            request.SetRequestHeader("x-api-key", "your-api-key-here"); // Если нужен API ключ

            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                try
                {
                    Debug.Log($"Собачки: {json}");

                    // Парсим JSON
                    DogItemDto[] dogsArray = JsonUtility.FromJson<DogItemsWrapper>($"{{\"dogs\":{json}}}").dogs;
                    List<DogItemDto> dogsList = new List<DogItemDto>(dogsArray);

                    Debug.Log($"Полученные породы собак: {dogsList.Count}");
                    return dogsList;
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
    public class DogItemsWrapper
    {
        public DogItemDto[] dogs;
    }

    [Serializable]
    public class DogItemDto
    {
        public string name;
        public int id;
    }
}