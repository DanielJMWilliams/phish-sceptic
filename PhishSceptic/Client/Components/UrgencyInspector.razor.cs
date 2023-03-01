using Microsoft.AspNetCore.Components;
using PhishSceptic.Client.Models;
using PhishSceptic.Client.Utilities;
using System.Net.Http.Json;

namespace PhishSceptic.Client.Components
{
    public partial class UrgencyInspector
    {
        [Inject] HttpClient HttpClient { get; set; }
        [Parameter] public EmailAnalyser emailAnalyser { get; set; }

        private string _emailBody = "";
        private string _emailTitle = "";

        private string[] urgentWords;
        private string[] threatWords;
        private string[] rewardWords;
        private string[] misspeltWords;
        private string[] allSuspiciousWords;

        private string[] _colors = { "mud-theme-info", "mud-theme-danger", "mud-theme-warning", "mud-theme-error" };
        //private string[] phishingWords;

        protected async override Task OnInitializedAsync()
        {
            await LoadWordCategories();
            if (wordCategories != null)
            {
                urgentWords = wordCategories.urgent;
                threatWords = wordCategories.threat;
                rewardWords = wordCategories.reward;
                misspeltWords = wordCategories.misspelling;
                allSuspiciousWords = urgentWords.Concat(threatWords).Concat(rewardWords).Concat(misspeltWords).ToArray();

            }

        }

        protected async override Task OnParametersSetAsync()
        {
            _emailBody = emailAnalyser.GetEmailBody();
            _emailBody = EmailAnalyser.StripOutLinks(_emailBody);
            _emailTitle = emailAnalyser.GetEmailTitle();

            //urgentWords = wordCategories

        }

        private WordCategories? wordCategories;

        public async Task LoadWordCategories()
        {
            try
            {
                wordCategories = await HttpClient.GetFromJsonAsync<WordCategories>("data/word_categories.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }
    }
}
