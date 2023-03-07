using Microsoft.AspNetCore.Components;
using PhishSceptic.Client.Utilities;

namespace PhishSceptic.Client.Components
{
    public partial class ResultsComponent
    {
        [Inject] IWarningService warningService { get; set; }
        public List<string> warnings = new List<string>();


        public void GenerateResults()
        {
            warnings = warningService.GetWarnings();
            StateHasChanged();
        }

    }
}
