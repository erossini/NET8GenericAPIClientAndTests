using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSC.CSharp.Library.APIClient.Interfaces
{
    /// <summary>
    /// Api Service Interface
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns></returns>
        Task<ApiResponse<TResponse>>? Get<TResponse>() where TResponse : class;

        /// <summary>
        /// Gets the specified query.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ApiResponse<TResponse>>? Get<TResponse>(string? query = null, JsonSerializerOptions? options = null) where TResponse : class;

        /// <summary>
        /// Gets the specified path.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ApiResponse<TResponse>>? Get<TResponse>(string? path = null, string? query = null, JsonSerializerOptions? options = null) where TResponse : class;

        /// <summary>
        /// Patches the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ApiResponse<TResponse>>? Patch<TRequest, TResponse>(TRequest? payload, string? query = null, JsonSerializerOptions? options = null) where TResponse : class;

        /// <summary>
        /// Patches the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="path">The path.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ApiResponse<TResponse>>? Patch<TRequest, TResponse>(TRequest? payload, string? path = null, string? query = null, JsonSerializerOptions? options = null) where TResponse : class;

        /// <summary>
        /// Posts the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="path">The path.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ApiResponse<TResponse>>? Post<TRequest, TResponse>(TRequest? payload, string? path = null, string? query = null, JsonSerializerOptions? options = null) where TResponse : class;

        /// <summary>
        /// Puts the specified payload.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="payload">The payload.</param>
        /// <param name="path">The path.</param>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<ApiResponse<TResponse>>? Put<TRequest, TResponse>(TRequest? payload, string? path = "", string? query = "", JsonSerializerOptions? options = null) where TResponse : class;
    }
}