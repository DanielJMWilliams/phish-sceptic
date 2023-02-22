using Microsoft.AspNetCore.Components.Forms;
using MimeKit;
using System.Text.RegularExpressions;

namespace PhishSceptic.Utilities
{
    public class EmailAnalyser
    {
        public IBrowserFile EmailFile { get; set; }

        private MimeMessage _mimeMessage;

        private string _emailBody = "";
        private string _sender = "";
        private List<string> _urls = new List<string>();
        private List<string> _domains = new List<string>();
        private List<string> _shortenedLinks = new List<string>();

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

        public async static Task<MimeMessage> MimeKitLoad(IBrowserFile emailFile)
        {
            Stream stream = emailFile.OpenReadStream();
            var path = @"..\tmp\" + emailFile.Name;
            FileStream fs = File.Create(path);
            await stream.CopyToAsync(fs);
            stream.Close();
            fs.Close();

            // Load a MimeMessage from a path
            var message = MimeMessage.Load(path);
            return message;
        }

        public async Task Analyse()
        {
            reset();
            _mimeMessage = await MimeKitLoad(EmailFile);

            //will only display first sender: minor issue
            _sender = _mimeMessage.From[0].ToString();

            _emailBody = _mimeMessage.TextBody;

            // extract all urls in email
            _urls = ExtractUrls(_emailBody);
            _domains = ExtractDomains(_urls) ;
            _shortenedLinks = ExtractShortenedDomains(GetDistinctDomains());


        }

        public List<string> GetShortenedDomains()
        {
            return _shortenedLinks;
        }

        public static List<string> ExtractShortenedDomains(List<string> domains)
        {
            string pattern = "(bit.ly)|(tinyurl.com)|(tiny.one)|(rotf.lol)|(rb.gy/diwcdb)";
            List<string> shortened = new List<string>();
            foreach (string domain in domains)
            {
                Match m = Regex.Match(domain, pattern);
                if (m.Success)
                {
                    string s = m.Groups[0].Value;

                    shortened.Add(s);
                }

            }
            return shortened;
        }

        public static List<string> ExtractDomains(List<string> urls)
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

        public static List<string> ExtractUrls(string emailString)
        {
            List<string> urls = new List<string>();
            // extract all urls in email
            // TODO: update pattern to extract urls broken by new line characters
            string pattern = @"https?://[a-zA-Z0-9\./-?=#]+";
            MatchCollection matches = Regex.Matches(emailString, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            
            foreach(Match m in matches)
            {
                urls.Add(m.Value);
            }
            return urls;

        }

        public string GetEmailBody()
        {
            return _emailBody;
        }
        public List<string> GetDistinctDomains()
        {
            return _domains.Distinct().ToList();
        }

        public List<string> GetUrls()
        {
            return _urls;
        }

        private string extractExtension(string filename)
        {
            string extension = extractSinglePattern(filename, @"\.(.*)").Groups[1].Value;
            return extension;

        }

        public string GetSender()
        {
            return _sender;
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
            _urls = new List<string>();
            _domains = new List<string>();
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
