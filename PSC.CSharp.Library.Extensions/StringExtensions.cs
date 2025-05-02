namespace PSC.CSharp.Library.Extensions
{
    /// <summary>
    /// String Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Builds the URI with path.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string BuildUriWithPath(this string baseUrl, string? path = null)
        {
            return BuildUri(baseUrl, path, null);
        }

        /// <summary>
        /// Builds the URI.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static string BuildUri(this string baseUrl, string? query = null)
        {
            return BuildUri(baseUrl, null, query);
        }

        /// <summary>
        /// Builds a valid Uri
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public static string BuildUri(this string baseUrl, string? path = null, string? query = null)
        {
            if (baseUrl == "/")
                baseUrl = "";

            if (baseUrl.Length > 1 && baseUrl.EndsWith('/'))
                baseUrl = baseUrl.Substring(baseUrl.Length - 1);

            if (!string.IsNullOrEmpty(path) && path.StartsWith('/'))
                if (path.Length > 1)
                    path = path.Substring(1, path.Length - 1);
                else
                    path = "";

            if (!string.IsNullOrEmpty(path) && path.EndsWith('/'))
                if (path.Length > 1)
                    path = path.Substring(0, path.Length - 1);
                else
                    path = "";

            if (!string.IsNullOrEmpty(query) && query.StartsWith('?'))
                query = query.Substring(1, query.Length - 1);

            string finalUri = $"{baseUrl}";
            if (!string.IsNullOrEmpty(path) && path.Length > 0)
                finalUri = finalUri + "/" + path;
            if (!string.IsNullOrEmpty(query) && query.Length > 0)
                finalUri += "?" + query;

            return string.IsNullOrEmpty(finalUri) ? "/" : finalUri;
        }

        /// <summary>
        /// Formats the URL.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string FormatUrl(this string str)
        {
            return str.Replace("//", "/");
        }
    }
}