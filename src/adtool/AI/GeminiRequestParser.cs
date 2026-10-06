using adtool.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace adtool.AI
{
    public class GeminiRequestParser : IRequestParser
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string Model = "gemini-3.1-flash-lite";

        public GeminiRequestParser(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public async Task<OnboardingRequest> ParseRequestAsync(string input)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={_apiKey}";
        
            var prompt = $@"
            Extract onboarding fields from this request and return ONLY valid JSON matching this schema: 
            {{""FirstName"": string, ""LastName"": string, ""Department"": string, ""OU"": string}}
            Request: ""{input}""";

            var payload = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                },
                generationConfig = new { responseMimeType = "application/json" }
            };

            var response = await _httpClient.PostAsJsonAsync(url, payload);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Gemini API call failed ({response.StatusCode}: {errorBody})");
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();

            string json = result.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

            var parsed = JsonSerializer.Deserialize<OnboardingRequest>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (parsed == null)
            {
                throw new InvalidOperationException("Gemini returned a response that could not be parsed as an OnboardingRequest.");
            }

            return parsed;
        }
    }
}