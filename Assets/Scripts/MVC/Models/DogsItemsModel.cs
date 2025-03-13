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
        private const string URL = "https://dogapi.dog/api/v2/breeds";

        public ReactiveCommand<List<DogItemDto>> DogList = new();

        public async void GetDogs()
        {
            var dogs = await GetBreeds();
            if (dogs != null)
            {
                DogList.Execute(dogs);
            }
        }

        private async UniTask<List<DogItemDto>> GetBreeds()
        {
            using UnityWebRequest request = UnityWebRequest.Get(URL);
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                try
                {
                    DogApiResponse response = JsonUtility.FromJson<DogApiResponse>($"{{\"wrapper\":{json}}}");
                    List<DogItemDto> dogsList = new List<DogItemDto>();

                    foreach (var dog in response.wrapper.data)
                    {
                        dogsList.Add(new DogItemDto
                        {
                            id = dog.id,
                            name = dog.attributes.name
                        });
                    }

                    return dogsList;
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

    // Обертка для корректного парсинга JSON
    [Serializable]
    public class DogApiResponse
    {
        public DogItemsWrapper wrapper;
    }

    [Serializable]
    public class DogItemsWrapper
    {
        public DogApiItem[] data;
    }

    [Serializable]
    public class DogApiItem
    {
        public string id; // ID - это строка в формате GUID
        public DogAttributes attributes;
    }

    [Serializable]
    public class DogAttributes
    {
        public string name;
    }

    [Serializable]
    public class DogItemDto
    {
        public string id;  // ID теперь строка (GUID)
        public string name;
    }
}
