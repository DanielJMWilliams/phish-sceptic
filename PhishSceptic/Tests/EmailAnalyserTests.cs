using Xunit;
using System.Collections.Generic;
using MimeKit;
using PhishSceptic.Client.Utilities;

namespace PhishScepticTests
{
    public class EmailAnalyserTests
    {
        [Fact]
        public void ExtractUrls()
        {
            string emailString = "";
         
            //no links
            List<string> actual = EmailAnalyser.ExtractUrls(emailString);
            List<string> expected = new List<string>();
            Assert.Equal(expected, actual);

            //extract only links starting with http or https
            emailString = "http://www.google.com https://google.com google.com www.google.com";
            actual = EmailAnalyser.ExtractUrls(emailString);
            expected = new List<string>()
            {
                "http://www.google.com",
                "https://google.com"
            };
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void ExtractDomains()
        {
            List<string> urls = new List<string>()
            {
                "https://www.dnswl.org/","https://www.linkedin.com/comm/onboarding/start/?midToken=3DAQFT=",
                "https://sub.linkedin.com/comm/onboarding/start/?midToken=3DAQFT=","https://bob.linkedin.com/comm/profile/edit?midT=",
                "https://www.linkedin.com/comm/mynetwork/?midToken=3DAQFT=", "https://www.linkedin.com/e/v2?e=3Ddne1zj"
            };
            List<string> actual = EmailAnalyser.ExtractDomains(urls);
            List<string> expected = new List<string>()
            {
                "dnswl.org","linkedin.com",
                "sub.linkedin.com","bob.linkedin.com", // subdomains are counted separately
                "linkedin.com", "linkedin.com"
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MimeKitLoad()
        {
            // Load a MimeMessage from a file path
            var message = MimeMessage.Load(@"C:\Users\Admin\Documents\1Uni\1FinalYear\Project\PhishScepticTests\PhishScepticTests\test-data\email.eml");
            string expected = "Hello Daniel,\r\n\r\nYou need to update your password now as your account has been compromised! Follow this link to change your password http://facebook.changemypassword.com.\r\n\r\nRegards,\r\n\r\nFacebook team.\r\n";
            Assert.Equal(expected, message.TextBody);

            var attachments = new List<MimePart>();
            var multiparts = new List<Multipart>();
            var iter = new MimeIterator(message);

            // collect our list of attachments and their parent multiparts
            while (iter.MoveNext())
            {
                var multipart = iter.Parent as Multipart;
                var part = iter.Current as MimePart;

                if (multipart != null && part != null && part.IsAttachment)
                {
                    // keep track of each attachment's parent multipart
                    multiparts.Add(multipart);
                    attachments.Add(part);
                }
            }

            // now remove each attachment from its parent multipart...
            for (int i = 0; i < attachments.Count; i++)
                multiparts[i].Remove(attachments[i]);
        }
    }
}