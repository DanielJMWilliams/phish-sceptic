using System.Net.Http.Json;

namespace PhishSceptic.Models
{
    public class QuestionBank
    {
        //use queue?
        private Question[]? questions;
        private readonly HttpClient _httpClient;

        public QuestionBank(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task LoadQuestions()
        {
            try
            {
                questions = await _httpClient.GetFromJsonAsync<Question[]>("data/questions.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        public Question[] GetQuestions()
        {
            if (questions!=null)
            {
                return questions;
            }
            throw new Exception("Questions object is null");
        }
    }
}
