using System;

namespace ResponseWrapper
{
    public interface IError
    {
        ProblemDetails GetProblemDetails(Exception exception);
    }

    public class ProblemDetails
    {
        public ProblemDetails(int code, string message, string developerErrorMessage = "")
        {
            Code = code;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            DeveloperErrorMessage = developerErrorMessage ?? throw new ArgumentNullException(nameof(developerErrorMessage));
        }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public string DeveloperErrorMessage { get; private set; }
    }
}
