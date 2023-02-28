using Microsoft.AspNetCore.Mvc;
using VirusTotalNet;
using VirusTotalNet.Results;

namespace PhishSceptic.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VirusTotalController : ControllerBase
    {
        private readonly ILogger<VirusTotalController> _logger;
        private readonly IConfiguration _config;

        public VirusTotalController(ILogger<VirusTotalController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpPost]

        [HttpGet("urlReport")]
        public async Task<string> Get(string url)
        {
            var apikey = _config["VirusTotal:ApiKey"];
            VirusTotal virusTotal = new VirusTotal(apikey);
            DomainReport domainReport = await virusTotal.GetDomainReportAsync(url);
            UrlReport urlReport = await virusTotal.GetUrlReportAsync(url);

            Console.WriteLine(domainReport);
            Console.WriteLine(domainReport.ResponseCode);
            return urlReport.Positives.ToString();
        }
    }
}
