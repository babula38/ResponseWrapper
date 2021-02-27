using Microsoft.AspNetCore.Mvc;
using ResponseWrapper.AspnetCore;

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
}
