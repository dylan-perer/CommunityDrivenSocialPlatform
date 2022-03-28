using Microsoft.Extensions.Logging;
using System;

namespace CDSP_API.Data
{
    public class EnityCoreResult
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorMsg { get; set; }
        public string InnerException { get; set; }
        public string InnerExceptionStackTrace { get; set; }

        public string ToString(object input)
        {
            return $"Error Message: {ErrorMsg}, Input: [{input}],Inner Exception: {InnerException}, Inner Exception StackTrace: {InnerExceptionStackTrace}";
        }

        public void MapException(Exception ex)
        {
            InnerException = ex.InnerException?.Message;
            ErrorMsg = ex.Message;
            InnerExceptionStackTrace = ex.StackTrace;
        }
    }
}
