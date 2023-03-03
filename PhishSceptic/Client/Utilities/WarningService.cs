namespace PhishSceptic.Client.Utilities
{
    public class WarningService : IWarningService
    {
        public List<string> Warnings { get; set; }

        public WarningService()
        {
            Warnings = new List<string>();
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        public List<string> GetWarnings()
        {
            Console.WriteLine("warnings: "+ Warnings.Count);
            return Warnings;
        }

        public void Reset()
        {
            Warnings = new List<string>();
        }
    }
}
