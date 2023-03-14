using MimeKit;

namespace PhishSceptic.Client.Utilities
{
    public interface IVirusTotalService
    {
        public Task<int> CheckReputation(string url);

        public Task<int> ScanFile(MimeEntity fileEntity);
    }
}
