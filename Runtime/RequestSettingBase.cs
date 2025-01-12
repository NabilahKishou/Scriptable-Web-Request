using System.Threading;
using System.Threading.Tasks;
using JeffreyLanters.WebRequests;
using UnityEngine;
using JeffreyLanters.WebRequests.Core;
#if UNITY_EDITOR
using NabilahKishou.ScriptableWebRequest.Editor;
#endif

namespace NabilahKishou.ScriptableWebRequest.Runtime {
    public abstract class RequestSettingBase : ScriptableObject {
        [Header("Basic Settings")] 
        public string endpoint;
        public RequestMethod method = RequestMethod.Get;
        public ContentType contentType = ContentType.ApplicationJson;
        public bool needAuth = false;
        public object body = null;

        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField] protected string finalUrl;
        protected IRequestBuilder requestBuilder;
        
        void OnValidate() {
            Url();
        }

        public virtual string Url() {
            return finalUrl = endpoint;
        }

        public virtual CustomWebRequest CreateRequest() {
            requestBuilder ??= new RequestBuilder().Construct(Url())
                .WithMethod(method)
                .WithContentType(contentType)
                .WithBody(body);
            return requestBuilder.WithAuth(needAuth).Build();
        }

        public virtual async Task<WebRequestResponse> SendRequest(CancellationToken cToken = default) {
            WebRequestResponse response = null;
            try {
                response = await CreateRequest().Send(cToken);
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

        protected string SubUrl(string sub) {
            return string.IsNullOrEmpty(sub) ? "" : "/" + sub;
        }
    }
}