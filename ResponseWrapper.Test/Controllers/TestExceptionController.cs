using Microsoft.AspNetCore.Mvc;
using ResponseWrapper.AspnetCore;

namespace ResponseWrapper.Test.Controllers
{
    [ApiResponse]
    [ApiController]
    [Route("[controller]")]
    public class TestExceptionController : ControllerBase
    {

        [HttpGet]
        public ActionResult<ApiDto> Get()
        {
            throw new CustomException();
        }
    }
}
