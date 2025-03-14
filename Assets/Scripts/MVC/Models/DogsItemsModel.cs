using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Custom;
using MVC.Abstract;
using UniRx;
using UnityEngine;
using Zenject;

namespace MVC.Models
{
    public class DogsItemsModel : Model
    {
        [Inject] private WebRequestService _webRequestService;

        private const string URL = "https://dogapi.dog/api/v2/breeds";

        private ConcurrentQueue<string> _requests = new();

        public ReactiveCommand<List<DogItemDto>> DogList = new();

        public void GetDogs()
        {
            GetBreeds();
        }

        public void CancelRequests()
        {
            foreach (var _ in _requests)
            {
                _requests.TryDequeue(out string id);
                _webRequestService.CancelRequest(id);
            }
        }

        private void GetBreeds()
        {
            var requestId = _webRequestService.EnqueueRequest(URL, OnSuccess, OnError);
            _requests.Enqueue(requestId);
        }

        private void OnSuccess(WebRequestDto responseJson)
        {
            try
            {
                _requests.TryDequeue(out _);

                DogApiResponse response = JsonUtility.FromJson<DogApiResponse>(responseJson.Response);
                List<DogItemDto> dogsList = new List<DogItemDto>();

                foreach (var dog in response.data)
                {
                    dogsList.Add(new DogItemDto
                    {
                        id = dog.id,
                        name = dog.attributes.name
                    });
                }

                DogList.Execute(dogsList);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка парсинга JSON: {ex.Message}\nJSON: {responseJson.Response}");
            }
        }

        private void OnError(WebRequestDto response)
        {
            _requests.TryDequeue(out _);

            Debug.LogError($"Error get breeds: {response.Response}");
        }
    }

    [Serializable]
    public class DogApiResponse
    {
        public DogApiItem[] data;
    }

    [Serializable]
    public class DogApiItem
    {
        public string id;
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
        public string id;
        public string name;
    }
}