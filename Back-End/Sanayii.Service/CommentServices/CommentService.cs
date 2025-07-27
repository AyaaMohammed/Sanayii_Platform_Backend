using System.Net.Http;
using System.Threading.Tasks;

public class CommentService
{
    private readonly HttpClient _httpClient;
    private static readonly List<string> OffensiveWords = new List<string> {
    "worst", "horrible","not happy", "trash", "awful", "terrible", "disappointing", "bad", "unacceptable",
    "pathetic", "rubbish", "useless", "hated", "poor", "dissatisfied", "inefficient",
    "horrendous", "subpar", "unprofessional", "horrible service", "unreliable", "frustrating",
    "regret", "stupid", "embarrassing", "slow", "failure", "unfriendly", "terrible experience",
    "waste of time", "poor quality", "disastrous", "annoying"
    };

    public CommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> FilterProfanityAsync(string text)
    {
        try
        {
            var encodedText = Uri.EscapeDataString(text);
            var response = await _httpClient.GetAsync($"https://www.purgomalum.com/service/containsprofanity?text={encodedText}");

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            // Log the response to see what is returned
            Console.WriteLine($"PurgoMalum API Response: {responseBody}");

            // Check if the response is "true" (indicating profanity)
            if (responseBody.Trim().ToLower() == "true")
            {
                return "true"; // Profanity detected
            }

            // Custom check for offensive words
            foreach (var word in OffensiveWords)
            {
                if (text.ToLower().Contains(word.ToLower()))
                {
                    return "true";  // Detected a bad comment
                }
            }

            return "false";  // Clean comment
        }
        catch (Exception ex)
        {
            // Log exception (you can use a logging framework here)
            return $"Error: {ex.Message}";
        }
    }
}
