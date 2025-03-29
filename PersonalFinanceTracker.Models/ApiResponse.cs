using System.Collections.Generic;

namespace PersonalFinanceTracker.Models
{
    /// <summary>
    /// Standard API response wrapper
    /// </summary>
    /// <typeparam name="T">Type of data being returned</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Whether the API call was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message providing additional information about the API call result
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The data returned by the API call
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Any errors that occurred during the API call
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Creates a successful response with data
        /// </summary>
        public static ApiResponse<T> CreateSuccess(T data, string message = "Operation completed successfully")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        /// <summary>
        /// Creates a failed response with errors
        /// </summary>
        public static ApiResponse<T> CreateError(string message, List<string> errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default(T),
                Errors = errors ?? new List<string>()
            };
        }
    }
}