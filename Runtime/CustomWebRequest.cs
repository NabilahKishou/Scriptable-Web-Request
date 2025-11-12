using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JeffreyLanters.WebRequests;
using JeffreyLanters.WebRequests.Core;
using UnityEngine.Networking;

namespace NabilahKishou.ScriptableWebRequest.Runtime {
    public class CustomWebRequest {
        public string URL { get; private set; }
        public RequestMethod method = RequestMethod.Get;
        public ContentType contentType = ContentType.ApplicationJson;
        public string characterSet = "utf-8";
        public object body = null;
        public List<Header> headers = new List<Header>();
        public List<QueryParameter> queryParameters = new List<QueryParameter>();
        
        public CustomWebRequest(string url) => this.URL = url;

        public async Task<WebRequestResponse> Send(CancellationToken cToken = default) {
            var isCompleted = false;
            var requestHandler = ToWebRequestHandler ();
            
            RoutineTicker.StartCompletableCoroutine (
                SendWebRequestHandler (requestHandler),
                () => isCompleted = true);
            while (isCompleted == false) {
                if (cToken.IsCancellationRequested)
                    cToken.ThrowIfCancellationRequested();
                await Task.Yield ();
            }
            
            if (requestHandler.result != WebRequestHandler.Result.Success)
                throw new WebRequestException (requestHandler);
            
            return new WebRequestResponse (requestHandler);
        }

        IEnumerator SendWebRequestHandler(WebRequestHandler requestHandler) {
            yield return requestHandler.SendWebRequest();
        }

        WebRequestHandler ToWebRequestHandler() {
            var handler = new WebRequestHandler ();
            handler.url = ParameterExtension.AppendQueryToUrl(URL, queryParameters);
            handler.method = this.method.ToString ().ToUpper ();
            
            handler.SetRequestHeader ("X-HTTP-Method-Override", handler.method);
            foreach (var header in this.headers) {
                handler.SetRequestHeader (header.name, header.value);
            }
            
            if ((this.method == RequestMethod.Post || this.method == RequestMethod.Put) 
                && this.body != null) {
                var encodedBody = Encoding.ASCII.GetBytes (this.body.ToString ());
                var type = this.contentType.Stringify ();
                type += $"; charset={this.characterSet}";
                
                if (this.contentType == ContentType.MultipartFormData) {
                    type += $"; boundary={FormDataUtility.boundary}";
                }
                handler.uploadHandler = new UploadHandlerRaw (encodedBody);
                handler.uploadHandler.contentType = type;
            }
            handler.downloadHandler = new DownloadHandlerBuffer ();
            return handler;
        }
    }
}
