using System.DirectoryServices.AccountManagement;
using adtool.Models;

namespace adtool.AD
{
    public class ADUserProvisioner : IUserProvisioner, IDisposable
    {
        private readonly string _domain;
        private readonly string _serviceAccountUser;
        private readonly string _serviceAccountPassword;

        public ADUserProvisioner(string domain, string serviceAccountUser, string serviceAccountPassword)
        {
            _domain = domain;
            _serviceAccountUser = serviceAccountUser;
            _serviceAccountPassword = serviceAccountPassword;
        }

        public bool UserExists(string samAccountName)
        {
            using var context = new PrincipalContext(ContextType.Domain, _domain, _serviceAccountUser, _serviceAccountPassword);
            using var existing = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);
            return existing != null;
        }

        public Results CreateUser(OnboardingRequest request, string username, string password)
        {
            try
            {
                using var context = new PrincipalContext(
                    ContextType.Domain,
                    _domain,
                    request.OU,
                    _serviceAccountUser,
                    _serviceAccountPassword
                );

                using var user = new UserPrincipal(context)
                {
                    SamAccountName = username,
                    GivenName = request.FirstName,
                    Surname = request.LastName,
                    DisplayName = $"{request.FirstName} {request.LastName}",
                    UserPrincipalName = $"{username}@{_domain}",
                    Enabled = true
                };

                user.SetPassword(password);
                user.ExpirePasswordNow();       // force change password at logon
                user.Save();

                return Results.Ok(username, user.DistinguishedName);
            } 
            catch (PrincipalExistsException)
            {
                return Results.Fail("$User '{username}' already exists.");
            } 
            catch (Exception ex)
            {
                return Results.Fail($"Unexpected error creating user: {ex.Message}");
            }
        }

        public void Dispose()
        {
            
        }
    }
}