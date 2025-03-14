using System;
using System.Collections.Concurrent;
using Custom;
using MVC.Abstract;
using UniRx;
using UnityEngine;
using Zenject;

namespace MVC.Models
{
    public class DogsPopUpModel : Model
    {
        [Inject] private WebRequestService _webRequestService;

        private const string BASE_URL = "https://dogapi.dog/api/v2/breeds";

        private ConcurrentQueue<string> _requests = new();

        public ReactiveCommand<string> DogDescription = new();

        public void GetDog(string id)
        {
            GetBreedDescription(id);
        }

        public void CancelLastRequest()
        {
            if (_requests.TryDequeue(out string id))
            {
                _webRequestService.CancelRequest(id);
            }
        }

        public void CancelRequests()
        {
            foreach (var _ in _requests)
            {
                _requests.TryDequeue(out string id);
                _webRequestService.CancelRequest(id);
            }
        }

        private void GetBreedDescription(string id)
        {
            string url = $"{BASE_URL}/{id}";

            var requestId = _webRequestService.EnqueueRequest(url, OnSuccess, OnError);
            _requests.Enqueue(requestId);
        }

        private void OnSuccess(WebRequestDto responseJson)
        {
            _requests.TryDequeue(out _);

            try
            {
                DogDescriptionResponse response = JsonUtility.FromJson<DogDescriptionResponse>(responseJson.Response);
                DogDescription.Execute(response.data.attributes.description);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка парсинга JSON: {ex.Message}\nJSON: {responseJson.Response}");
            }
        }

        private void OnError(WebRequestDto response)
        {
            _requests.TryDequeue(out _);

            Debug.LogError($"Error get temperature: {response.Response}");
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