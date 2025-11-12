using JeffreyLanters.WebRequests;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace NabilahKishou.ScriptableWebRequest.Runtime {
    public interface IRequestBuilder {
        CustomWebRequest Build();
        IRequestBuilder WithAuth(bool needAuth);
        IRequestBuilder WithMethod(RequestMethod method);
        IRequestBuilder WithContentType(ContentType contentType);
        IRequestBuilder WithBody(object body);
        IRequestBuilder WithCustomHeaders(params Header[] headers);
        IRequestBuilder WithQueryParams(params QueryParameter[] queryParameters);
    }
    
    public class RequestBuilder : IRequestBuilder {
        CustomWebRequest _request;

        public RequestBuilder(string url) {
            _request = new CustomWebRequest(url);
        }

        public CustomWebRequest Build() {
            return _request;
        }

        public IRequestBuilder WithAuth(bool isNeedAuth) {
            if (!isNeedAuth) return this;
            // var token = CredentialsPrefs.GetData(CredentialsDirectory.BEARER_TOKEN);
			var token = PlayerPrefs.GetString("bearer_token");
            _request.headers.Add(new Header("Authorization", $"Bearer {token}"));
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