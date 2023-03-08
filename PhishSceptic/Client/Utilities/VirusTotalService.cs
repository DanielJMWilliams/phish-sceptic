using Microsoft.AspNetCore.Components;
using MimeKit;
using System;
using System.Net.Http.Headers;

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

        public Task<int> ScanFile(FileStream file)
        {
            Console.WriteLine("scannin");
           throw new NotImplementedException();
           /* 
            var postBody = new { URL = url };
            //var response = await Http.PostAsJsonAsync("VirusTotal/urlReport", postBody);
            string positives = await http.GetStringAsync("VirusTotal/urlReport/positives?url=" + url);

            
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://www.virustotal.com/vtapi/v2/file/scan"),
                Headers =
    {
        { "accept", "text/plain" },
    },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "file", "data:text/plain;name=emailstring.txt;base64,77u/J1JlY2VpdmVkOiBmcm9tIExPMlAyNjVNQjU0NDguR0JSUDI2NS5QUk9ELk9VVExPT0suQ09NICgyNjAzOjEwYTY6NjAwOjI1Zjo6OSlcclxuIGJ5IENXWFAyNjVNQjU2NjkuR0JSUDI2NS5QUk9ELk9VVExPT0suQ09NIHdpdGggSFRUUFM7IFRodSwgOSBGZWIgMjAyM1xyXG4gMTE6MDY6MDEgKzAwMDBcclxuQVJDLVNlYWw6IGk9MjsgYT1yc2Etc2hhMjU2OyBzPWFyY3NlbGVjdG9yOTkwMTsgZD1taWNyb3NvZnQuY29tOyBjdj1wYXNzO1xyXG4gYj1GZ2JKMHAxVmwzRjN2U2F1RE5SUWUwd21mNHJmckUzL2xPUzJ5ejV5ajVvczFvMjZGMmxWUGZ0ZjNtYjJWN3VDdlhDUXdkam9aMWxmNWFtYjVLN285VC9GcFlKYmxWK0VoRGV6bW9wZWJNVE5nWDFGaEhiSnRONnQxYXRGSVk4Q0NuWjNESjV2VUFoYWZEK1VvV3I4U2hrWkF2aFRNd3hxY3dHc2VXVlRGcE5iTzkxSGZLbjllQzVWQW9vQUlhbGU3WVhZb3RyMXYxaW1MOXVScU5rYUdIUWtKdTdVeithbEg3cExzcnNmNFZOckN1QncvYUxGd2JpbVB2Yi9QSeKApnRhYmxlPlxyXG5cdFx0XHRcdFx0XHQ8L3RkPlxyXG5cdFx0XHRcdFx0PC90cj5cclxuXHRcdFx0XHQ8L3Rib2R5PlxyXG5cdFx0XHQ8L3RhYmxlPlxyXG5cdFx0XHQ8L2NlbnRlcj5cclxuXHRcdFx0PC90ZD5cclxuXHRcdDwvdHI+XHJcblx0PC90Ym9keT5cclxuPC90YWJsZT5cclxuXHJcbjxwPjxpbWcgYWx0PTNEIiIgcm9sZT0zRCJwcmVzZW50YXRpb24iIHNyYz0zRCJodHRwczovL3d3dy5saW5rZWRpbi5jb20vZW1pbT1cclxucC9pcF9Xa2MxYkUxWWNIRk1WM1JwV1hwYWQwMHpSbXRNV0Vad09scFhNV2hoVjNobVpESldjMWt5T1hSYVZqbHJXVmhzWmsxR09UPVxyXG5KTlp6MDlPZz0zRD0zRC5naWYiIHN0eWxlPTNEIm91dGxpbmU6bm9uZTstbXMtaW50ZXJwb2xhdGlvbi1tb2RlOmJpY3ViaWM7Y289XHJcbmxvcjojRkZGRkZGO3RleHQtZGVjb3JhdGlvbjpub25lO3dpZHRoOjFweDtoZWlnaHQ6MXB4OyI+PC9wPlxyXG48L2JvZHk+XHJcbjwvaHRtbD5cclxuXHJcbi0tZDIwNzEzYWRjYjY5NjE4Njc1NmJlMDg4ZDZiZTQzNzc4MGZlNTNiYmM1ZDhjYzMwZDE2ZmIyYWQ5NTQxLS1cclxuJw==" },
        { "apikey", "d954601fb0666f33d549ce981118388b0105aa8774ca3d2143a5c134cc87a1d4" },
    }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Console.WriteLine(body);
            }
        }
        */
            
        }
    }
}
