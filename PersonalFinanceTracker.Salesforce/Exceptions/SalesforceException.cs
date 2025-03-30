using System;
using System.Net;

namespace PersonalFinanceTracker.Salesforce.Exceptions
{
    /// <summary>
    /// Custom exception for Salesforce API errors.
    /// </summary>
    public class SalesforceException : Exception
    {
        /// <summary>
        /// The HTTP status code returned by Salesforce, if applicable.
        /// </summary>
        public HttpStatusCode? StatusCode { get; }

        /// <summary>
        /// The Salesforce error code, if available.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Creates a new SalesforceException with the specified message.
        /// </summary>
        public SalesforceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Creates a new SalesforceException with the specified message and inner exception.
        /// </summary>
        public SalesforceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a new SalesforceException with the specified message and status code.
        /// </summary>
        public SalesforceException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Creates a new SalesforceException with the specified message, status code, and Salesforce error code.
        /// </summary>
        public SalesforceException(string message, HttpStatusCode statusCode, string errorCode) : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates a new SalesforceException from a Salesforce API error response.
        /// </summary>
        public static SalesforceException FromApiError(HttpStatusCode statusCode, dynamic errorResponse)
        {
            try
            {
                // Handle standard Salesforce error format
                if (errorResponse != null && errorResponse.errorCode != null)
                {
                    string errorCode = errorResponse.errorCode.ToString();
                    string message = errorResponse.message != null ? errorResponse.message.ToString() : "Unknown Salesforce error";
                    return new SalesforceException(message, statusCode, errorCode);
                }
            }
            catch
            {
                // If we can't parse the error response, fall back to a generic error
            }

            return new SalesforceException($"Salesforce API error: {statusCode}", statusCode);
        }
    }
}
