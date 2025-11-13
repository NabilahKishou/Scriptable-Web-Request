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
        [SerializeField] protected string _finalUrl;
        
        [Header("Basic Settings")] 
        [SerializeField] protected string _endpoint;
        [SerializeField] protected RequestMethod _method = RequestMethod.Get;
        [SerializeField] protected ContentType _contentType = ContentType.ApplicationJson;
        [SerializeField] protected bool _needAuthorization;
        [SerializeField] protected List<Parameter> _queryParameters;
        protected object requestBody;
        
        public string Endpoint {
            get => _endpoint;
            set => _endpoint = value;
        }
        public object Body {
            get => requestBody;
            set => requestBody = value;
        }
        
        void OnValidate() => Url();

        public virtual string Url() => _finalUrl = ParameterExtension.AppendQueryToUrl(_endpoint, _queryParameters);
        
        public virtual CustomWebRequest CreateRequest() {
            return new RequestBuilder(Url())
                .WithMethod(_method)
                .WithContentType(_contentType)
                .WithBody(requestBody)
                .WithAuth(_needAuthorization)
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
    }
}