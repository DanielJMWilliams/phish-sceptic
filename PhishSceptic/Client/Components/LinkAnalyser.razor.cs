using Microsoft.AspNetCore.Components;
using MudBlazor;
using PhishSceptic.Client.Utilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VirusTotalNet;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;

namespace PhishSceptic.Client.Components
{
    public partial class LinkAnalyser
    {
        [Parameter] public EmailAnalyser emailAnalyser { get; set; }

        [Inject] HttpClient Http { get; set; }

        private List<string> urls;
        private List<string> domains;
        private List<string> shortenedDomains;
        private Color[] domainChipColors;

        protected async override Task OnParametersSetAsync()
        {
            urls = emailAnalyser.GetUrls();
            domains = emailAnalyser.GetDistinctDomains();
            shortenedDomains = emailAnalyser.GetShortenedDomains();
            domainChipColors = new Color[domains.Count()];
            for (int i = 0; i < domainChipColors.Length; i++)
            {
                domainChipColors[i] = Color.Default;
            }
        }


        public async Task CheckReputation(string url)
        {

            //string positives = await Http.GetStringAsync("VirusTotal");
            var postBody = new { URL = url };
            //var response = await Http.PostAsJsonAsync("VirusTotal/urlReport", postBody);
            string positives = await Http.GetStringAsync("VirusTotal/urlReport?url="+url);
            //string positives = await response.Content.ReadAsStringAsync();
            Console.WriteLine(positives);
            //Console.WriteLine(domainReport.ResponseCode);





        }
        private async Task VerifyDomain(int domainIndex)
        {

            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Verify that this domain really is what you think it is.");
            parameters.Add("TextToVerify", domains[domainIndex]);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await DialogService.ShowAsync<InspectionDialog>("Verify Domain", parameters, options);

            await CheckReputation(domains[domainIndex]);

            var result = await dialog.Result;

            if (result.Canceled)
            {
                //nothin
                // this needs to be here otherwise can get error when type casting result.Data to bool
                //if cancelled, there is no data
            }
            else if (!(bool)result.Data)
            {
                domainChipColors[domainIndex] = Color.Error;
            }
            else if ((bool)result.Data)
            {
                domainChipColors[domainIndex] = Color.Info;
            }
            StateHasChanged();
        }
    }
}
