using System;

namespace NabilahKishou.ScriptableWebRequest {
    public enum UrlDirectory {
        none,
        user,
        auth,
        utils,
    }

    public static class UrlDirectoryExtensions {
        public static string GetDirectory(UrlDirectory dir) {
            return dir switch {
                UrlDirectory.none => "",
                UrlDirectory.user => "user",
                UrlDirectory.auth => "auth",
                UrlDirectory.utils => "utils",
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
            };
        }
    }
}