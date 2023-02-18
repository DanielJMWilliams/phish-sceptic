namespace PhishSceptic.Models
{
    public class Question
    {
        public string question { get; set; }
        public string description { get; set; }
        public float weight { get; set; }

        public bool? isYes { get; set; }
    }
}
