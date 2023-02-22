using Microsoft.AspNetCore.Components;
using PhishSceptic.Utilities;

namespace PhishSceptic.Components
{
    public partial class LinkAnalyser
    {
        [Parameter] public EmailAnalyser emailAnalyser { get; set; }

        private List<string> urls;
        private List<string> domains;
        private List<string> shortenedDomains;

        protected async override Task OnParametersSetAsync()
        {
            urls = emailAnalyser.GetUrls();
            domains = emailAnalyser.GetDistinctDomains();
            shortenedDomains = emailAnalyser.GetShortenedDomains();
        }
    }
}
