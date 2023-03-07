using Microsoft.AspNetCore.Components;

namespace PhishSceptic.Client.Utilities
{
    public class VirusTotalService : IVirusTotalService
    {        
        private readonly HttpClient http;

        public VirusTotalService(HttpClient httpClient)
        {
            this.http = httpClient;
        }
        

        public async Task<int> CheckReputation(string url)
        {
            //string positives = await Http.GetStringAsync("VirusTotal");
            var postBody = new { URL = url };
            //var response = await Http.PostAsJsonAsync("VirusTotal/urlReport", postBody);
            string positives = await http.GetStringAsync("VirusTotal/urlReport/positives?url=" + url);
            if (int.TryParse(positives, out int result))
            {
                return result;
            }
            else
            {
                return -1;
            }


        }
    }
}
