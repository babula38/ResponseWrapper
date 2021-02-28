using Microsoft.AspNetCore.Mvc;
using ResponseWrapper.AspnetCore;
using System;

namespace ResponseWrapper.Test.Controllers
{
    [ApiResponse]
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet]
        public ActionResult<ApiDto> Get()
        {
            return new ApiDto { ID = 1, Value = "Test value" };
        }
    }

    public class ApiDto
    {
        public int ID { get; set; }
        public string Value { get; set; }
    }

    public class CustomException : Exception, IError
    {
        public CustomException() : base()
        //public CustomException(string message) : base(message)
        {

        }
        public ProblemDetails GetProblemDetails(Exception exception)
        {
            return new ProblemDetails(500, "Custom error", "Developer error message");
        }
    }
}
