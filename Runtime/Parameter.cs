using System;
using System.Collections.Generic;
using JeffreyLanters.WebRequests;

namespace NabilahKishou.ScriptableWebRequest.Runtime {
    [Serializable]
    public class Parameter {
        public string key;
        public string value;
    }

    public static class ParameterExtension {
        public static string AppendQueryToUrl(string url, List<Parameter> parameters) {
            if (parameters.Count <= 0) return url;

            var urlBuilder = "";
            if (!url.Contains("?")) urlBuilder += "?";
            for (int i = 0; i < parameters.Count; i++) {
                urlBuilder += $"{parameters[i].key}={parameters[i].value}";
                if (i < parameters.Count - 1) urlBuilder += "&";
            }

            return url + urlBuilder;
        }
        
        public static string AppendQueryToUrl(string url, List<QueryParameter> parameters) {
            if (parameters.Count <= 0) return url;

            var urlBuilder = "";
            if (!url.Contains("?")) urlBuilder += "?";
            for (int i = 0; i < parameters.Count; i++) {
                urlBuilder += $"{parameters[i].name}={parameters[i].value}";
                if (i < parameters.Count - 1) urlBuilder += "&";
            }

            return url + urlBuilder;
        }
    }
}