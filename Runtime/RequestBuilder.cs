using JeffreyLanters.WebRequests;
using UnityEngine;

namespace NabilahKishou.ScriptableWebRequest.Runtime {
    public interface IRequestBuilder {
        CustomWebRequest Build();
        IRequestBuilder WithAuth(bool needAuth, string token = "");
        IRequestBuilder WithMethod(RequestMethod method);
        IRequestBuilder WithContentType(ContentType contentType);
        IRequestBuilder WithBody(object body);
        IRequestBuilder WithCustomHeaders(params Header[] headers);
        IRequestBuilder WithQueryParams(params QueryParameter[] queryParameters);
    }
    
    public class RequestBuilder : IRequestBuilder {
        protected CustomWebRequest _request;

        public RequestBuilder(string url) {
            _request = new CustomWebRequest(url);
        }

        public CustomWebRequest Build() {
            return _request;
        }

        public virtual IRequestBuilder WithAuth(bool needAuth, string token = "") {
            if (!needAuth) return this;
            var bearerToken = string.IsNullOrEmpty(token) ? PlayerPrefs.GetString("bearer_token") : token;
            _request.headers.Add(new Header("Authorization", $"Bearer {bearerToken}"));
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
            _request.body = body;
            return this;
        }

        public IRequestBuilder WithCustomHeaders(params Header[] headers) {
            _request.headers.AddRange(headers);
            return this;
        }

        public IRequestBuilder WithQueryParams(params QueryParameter[] queryParameters) {
            _request.queryParameters.AddRange(queryParameters);
            return this;
        }
    }
}