using adtool.Models;

namespace adtool.AD
{
    public interface IUserProvisioner
    {
        Results CreateUser(OnboardingRequest request, string username, string password);
        bool UserExists(string samAccountName);
    }
}