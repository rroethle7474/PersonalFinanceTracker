using System;
using System.Collections.Generic;

namespace PersonalFinanceTracker.Models
{
        public class ApiErrorResponse
        {
            public string Message { get; set; }
            public string ExceptionMessage { get; set; }
            public string ExceptionType { get; set; }
            public string StackTrace { get; set; }
            public Dictionary<string, string[]> ModelState { get; set; }
            public DateTime Timestamp { get; set; }

            public ApiErrorResponse()
            {
                Timestamp = DateTime.UtcNow;
                ModelState = new Dictionary<string, string[]>();
            }
        }
}
