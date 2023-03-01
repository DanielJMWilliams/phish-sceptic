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
        private string[] domainChipIcons;

        protected async override Task OnParametersSetAsync()
        {
            urls = emailAnalyser.GetUrls();
            domains = emailAnalyser.GetDistinctDomains();
            shortenedDomains = emailAnalyser.GetShortenedDomains();
            domainChipColors = new Color[domains.Count()];
            domainChipIcons = new string[domains.Count()];
            for (int i = 0; i < domainChipColors.Length; i++)
            {
                domainChipColors[i] = Color.Default;
                domainChipIcons[i] = "";//Icons.Material.Filled.WarningAmber;//Icons.Material.Filled.Error
            }
        }


        public async Task<int> CheckReputation(string url)
        {

            //string positives = await Http.GetStringAsync("VirusTotal");
            var postBody = new { URL = url };
            //var response = await Http.PostAsJsonAsync("VirusTotal/urlReport", postBody);
            string positives = await Http.GetStringAsync("VirusTotal/urlReport/positives?url="+url);
            if(int.TryParse(positives, out int result))
            {
                return result;
            }
            else
            {
                return -1;
            }


        }
        private async Task VerifyDomain(int domainIndex)
        {

            var parameters = new DialogParameters();
            parameters.Add("ContentText", "Verify that this domain really is what you think it is.");
            parameters.Add("TextToVerify", domains[domainIndex]);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await DialogService.ShowAsync<InspectionDialog>("Verify Domain", parameters, options);


            var result = await dialog.Result;

            if (result.Canceled)
            {
                return;
            }

            int rep = await CheckReputation(domains[domainIndex]);
            if (rep == -1)
            {
                //unscanned
                domainChipColors[domainIndex] = Color.Warning;
                domainChipIcons[domainIndex] = Icons.Material.Filled.WarningAmber;
            }
            else if (rep > 0)
            {
                //scanned and bad
                domainChipColors[domainIndex] = Color.Error;
                domainChipIcons[domainIndex] = Icons.Material.Filled.Dangerous;
            }
            else if(rep == 0)
            {
                //scanned and fine
                domainChipColors[domainIndex] = Color.Info;
                domainChipIcons[domainIndex] = Icons.Material.Filled.Check;
            }
            else if (rep == -2)
            {
                //no response, rate limited
                domainChipColors[domainIndex] = Color.Warning;
                domainChipIcons[domainIndex] = "";
                Console.WriteLine("VirusTotal Rate Limit hit");
            }



            StateHasChanged();
        }
    }
}
