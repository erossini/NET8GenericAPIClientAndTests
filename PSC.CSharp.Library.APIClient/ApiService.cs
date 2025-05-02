using PSC.CSharp.Library.APIClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSC.CSharp.Library.APIClient
{
    /// <summary>
    /// Api Service base
    /// </summary>
    public class ApiService : IApiService
    {
        #region Variables

        /// <summary>
        /// The API key
        /// </summary>
        private string apiKey = string.Empty;

        /// <summary>
        /// The base endpoint
        /// </summary>
        private string baseEndpoint = string.Empty;

        /// <summary>
        /// The HTTP client
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// The logger
        /// </summary>
        private ILogger? logger;
        #endregion Variables
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiService"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="client">The client.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        public ApiService(string baseUrl, HttpClient client, ILogger logger)
        {
            this.logger = logger;
            httpClient = client;

            if (!string.IsNullOrEmpty(baseUrl))
                baseEndpoint = baseUrl;
            else
                throw new ArgumentNullException(nameof(baseUrl));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiService"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="apiKey">The API key.</param>
        /// <param name="client">The client.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="System.ArgumentNullException">apiKey</exception>
        public ApiService(string baseUrl, string apiKey, HttpClient client, ILogger logger)
        {
            this.logger = logger;
            httpClient = client;

            this.apiKey = apiKey;
            if (!string.IsNullOrEmpty(apiKey))
                httpClient.DefaultRequestHeaders.Add("Authorization", apiKey);
            else
                throw new ArgumentNullException(nameof(apiKey));

            if (!string.IsNullOrEmpty(baseUrl))
                baseEndpoint = baseUrl;
            else
                throw new ArgumentNullException(nameof(baseUrl));
        }

        #endregion

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns></returns>
        public async Task<ApiResponse<TResponse>>? Get<TResponse>()
            where TResponse : class
        {
            return await Get<TResponse>(null, null, null);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="query">The parameters.</param>
        /// <returns></returns>
        public async Task<ApiResponse<TResponse>>? Get<TResponse>(string? query = null, JsonSerializerOptions? options = null)
            where TResponse : class
        {
            return await Get<TResponse>(null, query, options);
        }

        /// <summary>
        /// Gets the specified path.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<ApiResponse<TResponse>>? Get<TResponse>(string? path = null, string? query = null, JsonSerializerOptions? options = null)
            where TResponse : class
        {
            if (!string.IsNullOrEmpty(path))
                path = "/" + path;
            if (!string.IsNullOrEmpty(query))
                query = "?" + query;

            string url = $"{baseEndpoint}".BuildUri(path, query);

            if (string.IsNullOrEmpty(url))
                return new ApiResponse<TResponse>() { Success = false, ErrorMessage = "Url non valid" };

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);

            try
            {
                responseMessage.EnsureSuccessStatusCode();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    WriteLog("GET", url.ToString(), "", responseMessage.StatusCode, null);
                    return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
                }
            }
            catch (HttpRequestException hex)
            {
                string hexText = string.Empty;
                TResponse rslHexText = default!;

                try
                {
                    hexText = await responseMessage.Content.ReadAsStringAsync();
                    rslHexText = await responseMessage.Content.ReadFromJsonAsync<TResponse>();
                }
                catch (Exception ex)
                {
                    WriteLog("GET", url.ToString(), "", responseMessage.StatusCode, ex,
                        description: "HttpRequestException Json Conversion");
                }

                WriteLog("GET", url.ToString(), "", responseMessage.StatusCode, hex, true, hexText);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode, Data = rslHexText };
            }
            catch (TaskCanceledException tex)
            {
                WriteLog("GET", url.ToString(), "", responseMessage.StatusCode, tex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }
            catch (Exception ex)
            {
                WriteLog("GET", url.ToString(), "", responseMessage.StatusCode, ex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }

            TResponse rsl = default!;
            try
            {
                if (responseMessage?.Content != null)
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    rsl = JsonSerializer.Deserialize<TResponse>(json, options);
                }
            }
            catch (Exception ex)
            {
                WriteLog("GET", url.ToString(), "Json Conversion", responseMessage.StatusCode, ex);
            }

            WriteLog("GET", url.ToString(), "", responseMessage.StatusCode, null, level: LogLevel.Information);
            return new ApiResponse<TResponse>() { Success = true, Data = rsl, HttpStatusCode = (int)responseMessage.StatusCode };
        }

        /// <summary>
        /// Patches the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<ApiResponse<TResponse>>? Patch<TRequest, TResponse>(TRequest? payload,
            string? query = null, JsonSerializerOptions? options = null)
            where TResponse : class
        {
            return await Patch<TRequest, TResponse>(payload, null, query, options);
        }

        /// <summary>
        /// Patches the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<ApiResponse<TResponse>>? Patch<TRequest, TResponse>(TRequest? payload, string? path = null,
            string? query = null, JsonSerializerOptions? options = null)
            where TResponse : class
        {
            if (payload == null)
                return new ApiResponse<TResponse>() { Success = false, ErrorMessage = "No valid payload." };

            string url = baseEndpoint.BuildUri(path, query);
            if (string.IsNullOrEmpty(url))
                return new ApiResponse<TResponse>() { Success = false, ErrorMessage = "Url non valid" };

            HttpResponseMessage responseMessage;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, url);

            string json = JsonSerializer.Serialize(payload, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;

            responseMessage = await httpClient?.SendAsync(request);

            try
            {
                responseMessage.EnsureSuccessStatusCode();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    WriteLog("PATCH", url.ToString(), "", responseMessage.StatusCode, null);
                    return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
                }
            }
            catch (HttpRequestException hex)
            {
                string hexText = string.Empty;
                TResponse rslHexText = default!;

                try
                {
                    hexText = await responseMessage.Content.ReadAsStringAsync();
                    rslHexText = await responseMessage.Content.ReadFromJsonAsync<TResponse>();
                }
                catch (Exception ex)
                {
                    WriteLog("PATCH", url.ToString(), "", responseMessage.StatusCode, ex,
                        description: "HttpRequestException Json Conversion");
                }

                WriteLog("PATCH", url.ToString(), "", responseMessage.StatusCode, hex, true, hexText);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode, Data = rslHexText };
            }
            catch (TaskCanceledException tex)
            {
                WriteLog("PATCH", url.ToString(), "", responseMessage.StatusCode, tex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }
            catch (Exception ex)
            {
                WriteLog("PATCH", url.ToString(), "", responseMessage.StatusCode, ex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }

            TResponse rsl = default!;
            try
            {
                if (responseMessage?.Content != null)
                    rsl = await responseMessage?.Content?.ReadFromJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                WriteLog("PATCH", url.ToString(), "Json Conversion", responseMessage.StatusCode, ex);
            }

            WriteLog("PATCH", url.ToString(), "", responseMessage.StatusCode, null, level: LogLevel.Information);
            return new ApiResponse<TResponse>() { Success = true, Data = rsl, HttpStatusCode = (int)responseMessage.StatusCode };
        }

        /// <summary>
        /// Posts the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns></returns>
        public async Task<ApiResponse<TResponse>>? Post<TRequest, TResponse>(TRequest? payload,
            string? path = null, string? query = null, JsonSerializerOptions? options = null)
            where TResponse : class
        {
            if (payload == null)
                return new ApiResponse<TResponse>() { Success = false, ErrorMessage = "No valid payload." };

            if (!string.IsNullOrEmpty(path))
                path = "/" + path;
            if (!string.IsNullOrEmpty(query))
                query = "?" + query;

            string url = $"{baseEndpoint}".BuildUri(path, query);
            if (string.IsNullOrEmpty(url))
                return new ApiResponse<TResponse>() { Success = false, ErrorMessage = "Url non valid" };

            HttpResponseMessage responseMessage;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

            string json = JsonSerializer.Serialize(payload, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;

            responseMessage = await httpClient.SendAsync(request);

            try
            {
                responseMessage.EnsureSuccessStatusCode();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    WriteLog("POST", url.ToString(), "", responseMessage.StatusCode, null);
                    return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
                }
            }
            catch (HttpRequestException hex)
            {
                string hexText = string.Empty;
                TResponse rslHexText = default!;

                try
                {
                    hexText = await responseMessage.Content.ReadAsStringAsync();
                    rslHexText = await responseMessage.Content.ReadFromJsonAsync<TResponse>();
                }
                catch (Exception ex)
                {
                    WriteLog("POST", url.ToString(), "", responseMessage.StatusCode, ex,
                        description: "HttpRequestException Json Conversion");
                }

                WriteLog("POST", url.ToString(), "", responseMessage.StatusCode, hex, true, hexText);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode, Data = rslHexText };
            }
            catch (TaskCanceledException tex)
            {
                WriteLog("POST", url.ToString(), "TaskCancellation", responseMessage.StatusCode, tex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }
            catch (Exception ex)
            {
                WriteLog("POST", url.ToString(), "", responseMessage.StatusCode, ex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }

            TResponse rsl = default!;
            try
            {
                if (responseMessage?.Content != null)
                    rsl = await responseMessage.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                WriteLog("POST", url.ToString(), "Json Conversion", responseMessage.StatusCode, ex);
            }

            WriteLog("POST", url.ToString(), "", responseMessage.StatusCode, null, level: LogLevel.Information);
            return new ApiResponse<TResponse>() { Success = true, Data = rsl, HttpStatusCode = (int)responseMessage.StatusCode };
        }

        /// <summary>
        /// Puts the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <returns></returns>
        public async Task<ApiResponse<TResponse>>? Put<TRequest, TResponse>(TRequest? payload, string? path = "",
            string? query = "", JsonSerializerOptions? options = null)
            where TResponse : class
        {
            if (payload == null)
                return new ApiResponse<TResponse>() { Success = false, ErrorMessage = "No valid payload." };

            string url = baseEndpoint.BuildUri(path, query);
            if (string.IsNullOrEmpty(url))
                return new ApiResponse<TResponse>() { Success = false, ErrorMessage = "Url non valid" };

            HttpResponseMessage responseMessage;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{url}");

            string json = JsonSerializer.Serialize(payload, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = content;

            responseMessage = await httpClient.SendAsync(request);

            try
            {
                responseMessage.EnsureSuccessStatusCode();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    WriteLog("PUT", url.ToString(), "", responseMessage.StatusCode, null);
                    return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
                }
            }
            catch (HttpRequestException hex)
            {
                string hexText = string.Empty;
                TResponse rslHexText = default!;

                try
                {
                    hexText = await responseMessage.Content.ReadAsStringAsync();
                    rslHexText = await responseMessage.Content.ReadFromJsonAsync<TResponse>();
                }
                catch (Exception ex)
                {
                    WriteLog("PUT", url.ToString(), "", responseMessage.StatusCode, ex,
                        description: "HttpRequestException Json Conversion");
                }

                WriteLog("PUT", url.ToString(), "", responseMessage.StatusCode, hex, true, hexText);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode, Data = rslHexText };
            }
            catch (TaskCanceledException tex)
            {
                WriteLog("PUT", url.ToString(), "TaskCancellation", responseMessage.StatusCode, tex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }
            catch (Exception ex)
            {
                WriteLog("PUT", url.ToString(), "", responseMessage.StatusCode, ex);
                return new ApiResponse<TResponse>() { Success = false, HttpStatusCode = (int)responseMessage.StatusCode };
            }

            TResponse rsl = default!;
            try
            {
                if (responseMessage.Content != null)
                    rsl = await responseMessage?.Content?.ReadFromJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                WriteLog("PUT", url.ToString(), "Json Conversion", responseMessage.StatusCode, ex);
            }

            WriteLog("PUT", url.ToString(), "", responseMessage.StatusCode, null, level: LogLevel.Information);
            return new ApiResponse<TResponse>() { Success = true, Data = rsl, HttpStatusCode = (int)responseMessage.StatusCode };
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <param name="fnzName">Name of the FNZ.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="isDebug">if set to <c>true</c> [is debug].</param>
        void WriteLog(string verb, string url, string fnzName, HttpStatusCode statusCode,
            Exception? ex, bool isDebug = true, string jsonError = null,
            string description = null,
            LogLevel level = LogLevel.Error)
        {
            if (logger != null)
            {
                string log = isDebug ? "[DBG]" : "";
                log = log + $"[HttpVerb: {verb}][URL: {url}]";
                log = log + (string.IsNullOrEmpty(fnzName) ? "" : $"[Funz: {fnzName}]");
                log = log + $"[HttpCode: {(int)statusCode}]";
                if (!string.IsNullOrEmpty(description))
                    log = log + $"[Description: {description}]";
                if (ex != null)
                    log = log + $"[Error: {ex.Message}][ErrSource: {ex.Source}][ErrStack: {ex.StackTrace}]";
                if (!string.IsNullOrEmpty(jsonError))
                    log = log + $"[HttpStream: {jsonError}]";

                switch (level)
                {
                    case LogLevel.Information:
                        logger?.LogInformation(log);
                        break;
                    case LogLevel.Warning:
                        logger?.LogWarning(log);
                        break;
                    default:
                        logger?.LogError(ex, log);
                        break;
                }
            }
        }
    }
}