using System;
using System.Collections.Generic;
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
        #if UNITY_EDITOR
        [ReadOnly]
        #endif
        [SerializeField] protected string finalUrl;
        
        [Header("Basic Settings")] 
        public string endpoint;
        public RequestMethod method = RequestMethod.Get;
        public ContentType contentType = ContentType.ApplicationJson;
        public bool needAuthorization = false;
        public List<Parameter> queryParameters;
        public object body = null;
        
        void OnValidate() {
            Url();
        }

        public virtual string Url() {
            return finalUrl = ParameterExtension.AppendQueryToUrl(endpoint, queryParameters);
        }

        public virtual CustomWebRequest CreateRequest() {
            return new RequestBuilder(Url())
                .WithMethod(method)
                .WithContentType(contentType)
                .WithBody(body)
                .WithAuth(needAuthorization)
                .Build();
        }

        public virtual async Task<WebRequestResponse> SendRequest(CancellationToken cToken = default) {
            WebRequestResponse response = null;
            try {
                response = await CreateRequest().Send(cToken);
            }
            catch (WebRequestException exception) {
                throw new Exception($"Error {exception.httpStatusCode} while fetching {exception.url}", exception);
            }
            
            return response;
        }

        public void SetBody(object body) {
            this.body = body;
        }

        protected string SubUrl(string sub) {
            return string.IsNullOrEmpty(sub) ? "" : "/" + sub;
        }
    }
}