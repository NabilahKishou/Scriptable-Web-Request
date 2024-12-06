using System.Collections.Generic;
using JeffreyLanters.WebRequests;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NabilahKishou.ScriptableWebRequest.Runtime {
    public interface IRequestBuilder {
        CustomWebRequest Build();
        IRequestBuilder Construct(string url);
        IRequestBuilder WithAuth(bool needAuth);
        IRequestBuilder WithMethod(RequestMethod method);
        IRequestBuilder WithContentType(ContentType contentType);
        IRequestBuilder WithBody(object body);
        IRequestBuilder WithCustomHeaders(Header[] headers);
        IRequestBuilder WithQueryParams(params QueryParameter[] queryParameters);
    }
    
    public class RequestBuilder : IRequestBuilder {
        CustomWebRequest _request;
        List<Header> _headers = new List<Header>();
        List<QueryParameter> _queryParameters = new List<QueryParameter>();

        public CustomWebRequest Build() {
            if (_headers.Count > 0) _request.headers = _headers.ToArray();
            if (_queryParameters.Count > 0) _request.queryParameters = _queryParameters.ToArray();
            return _request;
        }

        public IRequestBuilder Construct(string url) {
            _request = new CustomWebRequest(url);
            return this;
        }

        public IRequestBuilder WithAuth(bool isNeedAuth) {
            if (!isNeedAuth) return this;
            // var token = CredentialsPrefs.GetData(CredentialsDirectory.BEARER_TOKEN);
			var token = PlayerPrefs.GetString("bearer_token");
            _headers.Add(new Header("Authorization", $"Bearer {token}"));
            return this;
        }
        
        public IRequestBuilder WithMethod(RequestMethod method) {
            _request.method = method;
            return this;
        }

        public IRequestBuilder WithContentType(ContentType contentType) {
            _request.contentType = contentType;
            return this;
        }

        public IRequestBuilder WithBody(object body) {
            _request.body = body is JObject ? body : JsonUtility.ToJson(body);
            return this;
        }

        public IRequestBuilder WithCustomHeaders(Header[] headers) {
            _headers.AddRange(headers);
            return this;
        }

        public IRequestBuilder WithQueryParams(params QueryParameter[] queryParameters) {
            _queryParameters.AddRange(queryParameters);
            return this;
        }
    }
}