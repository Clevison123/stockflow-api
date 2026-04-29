using Microsoft.AspNetCore.Mvc;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings( GroupName = "Test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("StockFlow API is running successfully!");
        }
    }
}
