using adtool.Models;
using System.Threading.Tasks;

namespace adtool.AI
{
    public interface IRequestParser
    {
        Task<OnboardingRequest> ParseRequestAsync(string input);
    }
}