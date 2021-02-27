using System;
using System.Runtime.CompilerServices;

namespace ResponseWrapper
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data, OperationStatus status)
        {
            Data = data;
            Status = (string)status;
        }

        public string Status { get; private set; }
        public T Data { get; }

    }

    public class OperationStatus
    {
        string value;
        private OperationStatus(string value) => this.value = value;

        public static OperationStatus Success = new OperationStatus("Success");

        public static explicit operator string(OperationStatus status) => status.value;
    }
}
