using Microsoft.AspNetCore.Components;
using PhishSceptic.Utilities;

namespace PhishSceptic.Components
{
    public partial class LinkAnalyser
    {
        [Parameter] public EmailAnalyser emailAnalyser { get; set; }

        private List<string> urls;

        protected async override Task OnParametersSetAsync()
        {
            urls = emailAnalyser.GetUrls();
        }
    }
}
