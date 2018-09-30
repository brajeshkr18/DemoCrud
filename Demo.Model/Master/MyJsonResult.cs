using System;

namespace Demo.Model.Master
{
    /// <summary>
    /// Custom JSON Results
    /// </summary>
    public class MyJsonResult
    {

        /// <summary>
        /// class constructor
        /// </summary>
        public MyJsonResult()
        {

        }

        public string message { get; set; }
        public bool isSuccess { get; set; }

        public object data { get; set; }

        /// <summary>
        /// Create success object
        /// </summary>
        public static MyJsonResult CreateSuccess(string message)
        {
            return new MyJsonResult()
            {
                message = message,
                isSuccess = true
            };
        }

        /// <summary>
        /// Create error object for error message
        /// </summary>
        public static MyJsonResult CreateError(string message)
        {
            return new MyJsonResult()
            {
                message = message,
                isSuccess = false
            };
        }

        /// <summary>
        /// Create error object for exception message
        /// </summary>
        public static MyJsonResult CreateError(Exception ex)
        {
            return new MyJsonResult()
            {
                message = ex.Message,
                isSuccess = false,
                data = new { stacktrace = ex.StackTrace }
            };
        }
    }
}
