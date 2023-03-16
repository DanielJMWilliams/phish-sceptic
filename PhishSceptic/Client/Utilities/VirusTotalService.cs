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
            /*
                                 var resource = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("resource: " + resource);

                    string positives = await http.GetStringAsync("VirusTotal/fileScan/positives?resource=" + resource);
                    Console.WriteLine("Positives: " + positives);
                    return int.Parse(positives);
             */


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

                }
            }
        }
    }
}
