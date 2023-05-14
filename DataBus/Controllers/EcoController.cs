using Microsoft.AspNetCore.Mvc;

namespace DataBus.Controllers
{
    [Route("[controller]"), ApiController]
    public class EcoController : ControllerBase
    {
        [HttpGet("{input}")]
        public IActionResult Eco(string input) => Ok(input);

    }
}
