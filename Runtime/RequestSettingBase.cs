using Cysharp.Threading.Tasks;
using JeffreyLanters.WebRequests;
using JeffreyLanters.WebRequests.Core;
using NabilahKishou.ScriptableWebRequest.Runtime;
using UnityEditor;
using UnityEngine;

namespace NabilahKishou.ScriptableWebRequest {
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
                .WithAuth(needAuth)
                .WithMethod(method)
                .WithContentType(contentType)
                .WithBody(body);
            return requestBuilder.Build();
        }

        public virtual async UniTask<WebRequestResponse> SendRequest() {
            WebRequestResponse response = null;
            try {
                response = await CreateRequest().Send();
            } catch (WebRequestException exception) {
                Debug.Log ($"Error {exception.httpStatusCode} while fetching {exception.url}");
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
            return string.IsNullOrEmpty(sub)? "" : "/" + sub;
        }
    }
    
    #if UNITY_EDITOR
    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
            GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
        public override void OnGUI(Rect position,
            SerializedProperty property,
            GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    #endif
}