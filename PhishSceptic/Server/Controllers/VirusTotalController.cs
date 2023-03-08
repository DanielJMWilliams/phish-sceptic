using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        private VirusTotal virusTotal;

        public VirusTotalController(ILogger<VirusTotalController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            apikey = _config["VirusTotal:ApiKey"];
            virusTotal = new VirusTotal(apikey);
        }

        [HttpPost("fileScan")]
        public async Task<string> ScanFile(IFormFile file)
        {
            Console.WriteLine("scanfile api");
            // Check if a file was actually uploaded
            if (file == null || file.Length == 0)
            {
                Console.WriteLine("File is null or file.length is 0");
                return "bad request";
            }

            using (var ms = new MemoryStream())
            {
                // Read the uploaded file into a memory stream
                await file.CopyToAsync(ms);

                // Upload the file to VirusTotal for scanning
                ScanResult result = await virusTotal.ScanFileAsync(ms.ToArray(), file.FileName);
                Console.WriteLine("result: " + result);
                // Return the scan results as an HTTP response
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                //response.Content = new StringContent(result.Resource);
                response.Content = new StringContent("HELLO WORLD");
                Console.WriteLine("response: " + response);
                //return response;
                return result.Resource;
                
            }
        }

        [HttpGet("fileScan/positives")]
        public async Task<int> GetFileReport(string resource)
        {
            try
            {
                FileReport fileReport = await virusTotal.GetFileReportAsync(resource);
                Console.WriteLine("filelReport: " + fileReport);

                //if response not present because blocked by rate limiting
                if (fileReport.ResponseCode == FileReportResponseCode.NotPresent)
                {
                    return -2;
                }

                //If total=0, then it wasnt set because the requested url has not been scanned before
                if (fileReport.Total == 0)
                {
                    //for previously unscanned urls, return -1
                    return -1;
                }
                //otherwise return number of positives
                return fileReport.Positives;
            }
            catch (Exception ex)
            {
                //error;
                Console.WriteLine("Error: " + ex.Message);
                return -2;
            }

        }

        [HttpGet("urlReport/positives")]
        public async Task<int> Get(string url)
        {
            try
            {
                UrlReport urlReport = await virusTotal.GetUrlReportAsync(url);
                Console.WriteLine("urlReport: " + urlReport);

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
                Console.WriteLine("Error: " + ex.Message);
                return -2;
            }

        }
    }
}
