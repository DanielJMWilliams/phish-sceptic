using Microsoft.AspNetCore.Mvc;
using VirusTotalNet;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;

namespace PhishSceptic.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VirusTotalController : ControllerBase
    {
        private readonly ILogger<VirusTotalController> _logger;
        private readonly IConfiguration _config;
        private string apikey = "Not Set";

        public VirusTotalController(ILogger<VirusTotalController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            apikey = _config["VirusTotal:ApiKey"];
        }

        [HttpPost]

        [HttpGet("urlReport/positives")]
        public async Task<int> Get(string url)
        {
            VirusTotal virusTotal = new VirusTotal(apikey);
            //DomainReport domainReport = await virusTotal.GetDomainReportAsync(url);
            try
            {
                UrlReport urlReport = await virusTotal.GetUrlReportAsync(url);

                //if response not present because blocked by rate limiting
                if(urlReport.ResponseCode == UrlReportResponseCode.NotPresent)
                {
                    return -2;
                }

                //If total=0, then it wasnt set because the requested url has not been scanned before
                if (urlReport.Total == 0)
                {
                    //for previously unscanned urls, return -1
                    return -1;
                }
                //otherwise return number of positives
                return urlReport.Positives;
            }
            catch (Exception ex)
            {
                //error;
                return -2;
            }

        }
    }
}
