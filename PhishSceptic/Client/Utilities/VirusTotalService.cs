using Microsoft.AspNetCore.Components;
using MimeKit;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

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


        private async Task<FileStream> toFileStream(MimeEntity fileEntity)
        {
            Console.WriteLine("Scan time");
            FileStream stream;
            if (fileEntity is MessagePart)
            {
                var fileName = fileEntity.ContentDisposition?.FileName;
                var rfc822 = (MessagePart)fileEntity;

                if (string.IsNullOrEmpty(fileName))
                    fileName = "attached-message.eml";

                stream = File.Create(fileName);
                Console.WriteLine("Scan 1");
                rfc822.Message.WriteTo(stream);
            }
            else
            {
                var part = (MimePart)fileEntity;
                var fileName = part.FileName;

                stream = File.Create(fileName);
                
                Console.WriteLine("Scan 2");
                part.Content.DecodeTo(stream);
            }
            return stream;
        }
        public async Task<int> ScanFile(MimeEntity fileEntity)
        {
            Console.WriteLine("Scanning");

            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                await fileEntity.WriteToAsync(memoryStream);

                fileBytes = memoryStream.ToArray();
            }

            using (var streamContent = new ByteArrayContent(fileBytes))
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(streamContent, "file", fileEntity.ContentType.Name);

                    var response = await http.PostAsync("VirusTotal/fileScan", formData);
                    

                    var resource = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("resource: " + resource);

                    string positives = await http.GetStringAsync("VirusTotal/fileScan/positives?resource=" + resource);
                    Console.WriteLine("Positives: " + positives);
                    return int.Parse(positives);

                    //string positives = await http.GetStringAsync("VirusTotal/urlReport/positives?resource=" + resource);

                    /*
                    // Check the response status code
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("success");
                        // Get the response content
                        var responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("responseContent: " + responseContent);

                        // Deserialize the response content to a JSON object
                        var jsonObject = JObject.Parse(responseContent);

                        Console.WriteLine("jsonObject: " + jsonObject.ToString());

                        // Get the number of positive detections for the scanned file
                        //var positives = (int)jsonObject["data"]["attributes"]["last_analysis_stats"]["malicious"];

                        // Return the number of positive detections
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("failed");
                        // The request failed, return -1
                        return -1;
                    }
                    */
                }
            }
        }
    }
}
