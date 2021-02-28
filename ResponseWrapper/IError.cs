using System;

namespace ResponseWrapper
{
    public interface IError
    {
        ProblemDetails GetProblemDetails(Exception exception);
    }

    public class ProblemDetails
    {
        public ProblemDetails(string code, string message, string developerErrorMessage = "")
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            DeveloperErrorMessage = developerErrorMessage ?? throw new ArgumentNullException(nameof(developerErrorMessage));
        }

        public string Code { get; private set; }

        public string Message { get; private set; }

        public string DeveloperErrorMessage { get; private set; }
    }
}
