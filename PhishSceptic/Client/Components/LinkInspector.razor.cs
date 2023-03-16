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
    public partial class LinkInspector
    {
        [Parameter] public EmailAnalyser emailAnalyser { get; set; }

        [Inject] ISnackbar Snackbar { get; set; }

        [Inject] HttpClient Http { get; set; }
        [Inject] IVirusTotalService vtService { get; set; }
        [Inject] IWarningService warningService { get; set; }


        private List<string> urls;
        private List<string> domains;
        private List<string> shortenedDomains;
        private List<string> urlsContainingAnchors;

        protected async override Task OnParametersSetAsync()
        {
            urls = emailAnalyser.GetUrls();
            urlsContainingAnchors = emailAnalyser.GetUrlsContainingAnchors();
            domains = emailAnalyser.GetDistinctDomains();
            shortenedDomains = emailAnalyser.GetShortenedDomains();
        }

    }
}
