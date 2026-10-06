using adtool.Models;

namespace adtool.AI
{
    public interface IRequestParser
    {
        Task<OnboardingRequest> ParseRequestAsync(string input);
    }
}