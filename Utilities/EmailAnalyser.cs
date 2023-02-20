using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;

namespace PhishSceptic.Utilities
{
    public class EmailAnalyser
    {
        public IBrowserFile EmailFile { get; set; }

        // email as a bitstring
        private string _emailString = "";
        private string emailBody = "";
        private string Sender = "";
        private List<string> Urls = new List<string>();
        private List<string> Domains = new List<string>();

        public EmailAnalyser(IBrowserFile file)
        {
            Console.WriteLine(extractExtension(file.Name));
            if(file != null && extractExtension(file.Name)=="eml")
            {
                EmailFile = file;
            }
            else
            {
                Console.WriteLine("file is null or not a .eml file");
                throw new Exception("file is null or not a .eml file");
            }
            

        }

        public string GetEmailBody()
        {
            return emailBody;
        }

        public List<string> GetUrls()
        {
            return Urls;
        }

        private string extractExtension(string filename)
        {
            string extension = extractSinglePattern(filename, @"\.(.*)").Groups[1].Value;
            return extension;

        }

        public string GetSender()
        {
            return Sender;
        }

        public IBrowserFile GetFile()
        {
            return EmailFile;
        }

        public string GetFileName()
        {
            return EmailFile.Name;
        }

        public long GetFileSize()
        {
            return EmailFile.Size;
        }

        private void reset()
        {
            Urls = new List<string>();
            Domains = new List<string>();
        }

        public async Task Analyse()
        {
            reset();
            var buffer = new byte[EmailFile.Size];
            var length = await EmailFile.OpenReadStream().ReadAsync(buffer);

            using (var streamReader = new StreamReader(EmailFile.OpenReadStream()))
            {
                _emailString = await streamReader.ReadToEndAsync();
            }

            // extract sender
            string senderPattern = @"From: ([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})";
            Sender = extractSinglePattern(_emailString, senderPattern).Groups[1].Value;

            // extract all urls in email
            // TODO: update pattern to extract urls broken by new line characters
            string urlPattern = @"https?://[a-zA-Z0-9\./-?=#]+";
            MatchCollection urlMatches = extractPattern(_emailString, urlPattern);
            foreach (Match m in urlMatches)
            {
                Urls.Add(m.Groups[0].Value);
            }
            Domains = extractDomains(Urls);

            // extract body of email
            emailBody = extractBody(_emailString);



        }

        public string extractBody(string emailString)
        {
            // Use regular expression to extract body of email
            Regex bodyRegex = new Regex("\n\n(.+)", RegexOptions.Singleline);
            Match match = bodyRegex.Match(emailString);

            if (match.Success)
            {
                string body = match.Groups[1].Value;
                return body;
                Console.WriteLine(body);
            }
            else
            {
                Console.WriteLine("No body found in email");
                return "empty";
            }
        }

        private List<string> extractDomains(List<string> urls)
        {
            // Not perfect
            string pattern = @"(?:https?:\/\/)?(?:[^@\n]+@)?(?:www\.)?([^:\/\n]+)";
            List<string> domains = new List<string>();
            foreach (string url in urls)
            {
                GroupCollection groups = Regex.Match(url, pattern).Groups;
                string domain = groups[1].Value;

                domains.Add(domain);
            }
            return domains;
        }

        private Match extractSinglePattern(string text, string pattern)
        {
            Match match = Regex.Match(text, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return match;
            }

            throw new Exception("No match found for given pattern");
        }
        private MatchCollection extractPattern(string text, string pattern)
        {
            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return matches;
        }
    }
}
