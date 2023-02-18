namespace PhishSceptic.Utilities
{
    public class Question
    {
        public string question { get; set; }
        public float weight { get; set; }
        public string description { get; set; }

        public bool? isCorrect { get; set; }
    }
}
