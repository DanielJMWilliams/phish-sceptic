namespace PhishSceptic.Client.Utilities
{
    public interface IVirusTotalService
    {
        public Task<int> CheckReputation(string url);
    }
}
