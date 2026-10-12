using adtool.AD;
using adtool.AI;
using adtool.Models;
using adtool.JSONCheck;

namespace adtool
{
    public class Program{
        public static async Task Main(string[] args)
        {
            var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("Error: GEMINI_API_KEY environment variable not set.");
                return;
            }

            var adDomain = "szhu.lab";
            var adServiceUser = Environment.GetEnvironmentVariable("AD_SERVICE_USER");
            var adServicePassword = Environment.GetEnvironmentVariable("AD_SERVICE_PASSWORD");

            if (string.IsNullOrWhiteSpace(adServiceUser))
            {
                Console.WriteLine("Error: AD_SERVICE_USER environment variable not set");
                return;
            }

            var httpClient = new HttpClient();
            IRequestParser parser = new GeminiRequestParser(httpClient, apiKey);

            var validOUs = new HashSet<string>
            {
                "OU=Test,DC=szhu,DC=lab",
            };
            
            var validator = new RequestValidator(validOUs);

            var passwordGen = new PasswordGenerator();
            var existingUsers = new HashSet<string> {"jsmith"};

            var provisioner = new ADUserProvisioner(adDomain, adServiceUser, adServicePassword);

            var usernameGen = new UsernameGenerator(u => existingUsers.Contains(u));

            Console.WriteLine("Enter an onboarding request (e.g. \"Onboard Jane Doe, Marketing, OU=Marketing,DC=lab,DC=local\"):");
            string input = Console.ReadLine();

            OnboardingRequest request;
            try
            {
                request = await parser.ParseRequestAsync(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to parse request: {ex.Message}");
                return;
            }

            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                Console.WriteLine("Request failed validation:");
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine($"    - {error}");
                }
                return;
            }

            string username = usernameGen.Generate(request.FirstName, request.LastName);
            string password = passwordGen.Generate();

            Console.WriteLine();
            Console.WriteLine("Request validated. Would provision:");
            Console.WriteLine($"  Name:       {request.FirstName} {request.LastName}");
            Console.WriteLine($"  Department: {request.Department}");
            Console.WriteLine($"  OU:         {request.OU}");
            Console.WriteLine($"  Username:   {username}");
            Console.WriteLine($"  Password:   {password}");
            Console.WriteLine();
            Console.Write("Proceed with AD write (y/n)\n");

            if(Console.ReadLine()?.Trim().ToLower() != "y")
            {
                Console.WriteLine("Operation could not be performed. Try again.");
                return;
            }

            var result = provisioner.CreateUser(request, username, password);

            if (result.Success)
            {
                Console.WriteLine($"Success - created {result.Username} at {result.DistinguishedName}");
            } else
            {
                Console.WriteLine($"Failed: {result.ErrorMessage}");
            }
        }
    }
    

    
}

