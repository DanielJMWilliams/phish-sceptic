using Microsoft.AspNetCore.Components;
using MudBlazor;
using PhishSceptic.Client.Utilities;

namespace PhishSceptic.Client.Components
{
    public partial class UrlInspector
    {
        [Parameter] public EmailAnalyser emailAnalyser { get; set; }

        [Inject] ISnackbar Snackbar { get; set; }

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
            if (shortenedDomains.Count() > 0)
            {
                warningService.AddWarning("Email contains shortened links");
            }
            if (urlsContainingAnchors.Count() > 0) {
                warningService.AddWarning("Email contains URLs with anchors");
            }
        }

    }
}
