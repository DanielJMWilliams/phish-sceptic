namespace PhishSceptic.Client.Models
{
    public class Question
    {
        public string question { get; set; }
        public string description { get; set; }
        public float weighting { get; set; }

        public bool? isYes { get; set; }
    }
}
