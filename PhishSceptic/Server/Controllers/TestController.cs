using Microsoft.AspNetCore.Mvc;

namespace PhishSceptic.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IConfiguration _config;

        public TestController(ILogger<TestController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public string Get()
        {
            //var apikey = _config["VirusTotal:ApiKey"];
            return "Online";
        }
    }
}
