namespace PSC.CSharp.Library.APIClient
{
    /// <summary>
    /// Api Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> where T : class
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the HTTP error code.
        /// </summary>
        /// <value>
        /// The HTTP error code.
        /// </value>
        [JsonPropertyName("httpStatusCode")]
        public int HttpStatusCode { get; set; } = 200;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApiResponse{T}"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;
    }
}