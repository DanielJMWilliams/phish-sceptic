namespace PhishSceptic.Client.Utilities
{
    public interface IWarningService
    {
        public void AddWarning(string warning);

        public List<string> GetWarnings();

        public void Reset();
    }
}
