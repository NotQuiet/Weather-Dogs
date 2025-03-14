using System;
using System.Collections.Concurrent;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Zenject;

namespace Custom
{
    public class WebRequestService : IDisposable
    {
        [Inject] private EventBus _eventBus;
        
        private readonly ConcurrentQueue<WebRequestItem> _requestQueue;
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _requestTokens;
        private readonly CancellationTokenSource _serviceCancellationTokenSource;
        private bool _isProcessing;
        private bool _isDisposed;

        public WebRequestService()
        {
            _requestQueue = new ConcurrentQueue<WebRequestItem>();
            _requestTokens = new ConcurrentDictionary<string, CancellationTokenSource>();
            _serviceCancellationTokenSource = new CancellationTokenSource();
        }

        public string EnqueueRequest(string url,
            Action<WebRequestDto> successCallback,
            Action<WebRequestDto> errorCallback,
            CancellationToken? externalCancellationToken = null)
        {
            string requestId = Guid.NewGuid().ToString();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(
                externalCancellationToken ?? CancellationToken.None,
                _serviceCancellationTokenSource.Token
            );

            UnityWebRequest webRequest = UnityWebRequest.Get(url);

            var item = new WebRequestItem(
                requestId,
                webRequest,
                successCallback,
                errorCallback,
                cts.Token
            );

            _requestQueue.Enqueue(item);
            _requestTokens.TryAdd(requestId, cts);

            if (!_isProcessing)
            {
                _isProcessing = true;
                ProcessQueueAsync().Forget();
            }

            return requestId;
        }

        public bool CancelRequest(string requestId)
        {
            if (_requestTokens.TryRemove(requestId, out CancellationTokenSource cts))
            {
                cts.Cancel();
                cts.Dispose();
                return true; 
            }

            return false;
        }

        private async UniTaskVoid ProcessQueueAsync()
        {
            while (!_serviceCancellationTokenSource.IsCancellationRequested && !_requestQueue.IsEmpty)
            {
                if (_requestQueue.TryDequeue(out WebRequestItem request))
                {
                    if (request.CancellationToken.IsCancellationRequested)
                    {
                        if (_requestTokens.TryRemove(request.Id, out CancellationTokenSource cts))
                        {
                            cts.Dispose();
                        }

                        request.WebRequest.Dispose();
                        continue;
                    }

                    try
                    {
                        string result = await ProcessRequestAsync(request);
                        if (!request.CancellationToken.IsCancellationRequested)
                        {
                            request.SuccessCallback?.Invoke(new WebRequestDto(request.Id, result));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!request.CancellationToken.IsCancellationRequested)
                        {
                            request.ErrorCallback?.Invoke(new WebRequestDto(request.Id, ex.Message));
                        }
                    }
                    finally
                    {
                        if (_requestTokens.TryRemove(request.Id, out CancellationTokenSource cts))
                        {
                            cts.Dispose();
                        }

                        request.WebRequest.Dispose();
                    }
                }
            }

            _isProcessing = false;
        }

        private async UniTask<string> ProcessRequestAsync(WebRequestItem request)
        {
            _eventBus.OnStartLoading.Execute();
            
            var operation = request.WebRequest.SendWebRequest();
            await operation.ToUniTask(
                Progress.Create<float>(_ => { }),
                PlayerLoopTiming.Update,
                request.CancellationToken,
                false
            );

            if (request.WebRequest.result == UnityWebRequest.Result.Success)
            {
                _eventBus.OnEndLoading.Execute();
                return request.WebRequest.downloadHandler.text;
            }
            else
            {
                _eventBus.OnEndLoading.Execute();
                throw new Exception($"Web request failed: {request.WebRequest.error}");
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _serviceCancellationTokenSource.Cancel();

            foreach (var cts in _requestTokens.Values)
            {
                cts.Cancel();
                cts.Dispose();
            }

            _requestTokens.Clear();

            while (_requestQueue.TryDequeue(out WebRequestItem item))
            {
                item.WebRequest.Dispose();
            }

            _serviceCancellationTokenSource.Dispose();
            _isDisposed = true;
        }
    }

    public class WebRequestItem
    {
        public string Id { get; }
        public UnityWebRequest WebRequest { get; }
        public Action<WebRequestDto> SuccessCallback { get; }
        public Action<WebRequestDto> ErrorCallback { get; }
        public CancellationToken CancellationToken { get; }

        public WebRequestItem(string id, UnityWebRequest webRequest,
            Action<WebRequestDto> successCallback,
            Action<WebRequestDto> errorCallback,
            CancellationToken cancellationToken)
        {
            Id = id;
            WebRequest = webRequest;
            SuccessCallback = successCallback;
            ErrorCallback = errorCallback;
            CancellationToken = cancellationToken;
        }
    }

    public class WebRequestDto
    {
        public string Id { get; private set; }
        public string Response { get; private set; }

        public WebRequestDto(string id, string response)
        {
            Id = id;
            Response = response;
        }
    }
}