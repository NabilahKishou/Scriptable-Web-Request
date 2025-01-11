using Cysharp.Threading.Tasks;
using JeffreyLanters.WebRequests;
using JeffreyLanters.WebRequests.Core;
using NabilahKishou.ScriptableWebRequest.Editor;
using NabilahKishou.ScriptableWebRequest.Runtime;
using UnityEditor;
using UnityEngine;

namespace NabilahKishou.ScriptableWebRequest.Runtime {
    public abstract class RequestSettingBase : ScriptableObject {
        protected const string URL_PREFIX = "https://therapy.digiyata.com/api";

        [Header("Basic Settings")] 
        public string endpoint;
        public UrlDirectory directory = UrlDirectory.none;
        public RequestMethod method = RequestMethod.Get;
        public ContentType contentType = ContentType.ApplicationJson;
        public bool needAuth = false;
        public object body = null;

        [ReadOnly, SerializeField] protected string finalUrl;
        protected IRequestBuilder requestBuilder;

        public virtual string Url() {
            return finalUrl = $"{URL_PREFIX}" +
                              $"{SubUrl(UrlDirectoryExtensions.GetDirectory(directory))}" +
                              $"{SubUrl(endpoint)}";
        }

        public virtual CustomWebRequest CreateRequest() {
            requestBuilder ??= new RequestBuilder().Construct(Url())
                .WithMethod(method)
                .WithContentType(contentType)
                .WithBody(body);
            return requestBuilder.WithAuth(needAuth).Build();
        }

        public virtual async UniTask<WebRequestResponse> SendRequest() {
            WebRequestResponse response = null;
            try {
                response = await CreateRequest().Send();
            }
            catch (WebRequestException exception) {
                Debug.LogError($"Error {exception.httpStatusCode} while fetching {exception.url}");
                throw;
            }

            return response;
        }

        public void SetBody(object body) {
            this.body = body;
            requestBuilder?.WithBody(this.body);
        }

        void OnValidate() {
            Url();
        }

        protected string SubUrl(string sub) {
            return string.IsNullOrEmpty(sub) ? "" : "/" + sub;
        }
    }
}