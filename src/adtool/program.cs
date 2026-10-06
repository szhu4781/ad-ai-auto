using adtool.AD;
using adtool.AI;
using adtool.Models;
using adtool.JSONCheck;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace adtool
{
    public class Program{
        public static async Task Main(string[] args)
        {
            var apiKey = Environment.GetEnvironmentVariable("API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("Error: Gemini API Key environment variable not set.");
                return;
            }

            var httpClient = new HttpClient();
            IRequestParser parser = new GeminiRequestParser(httpClient, apiKey);

            var validOUs = new HashSet<string>
            {
                "OU=Sales,DC=lab,DC=local",
                "OU=Engineering,DC=lab,DC=local",
                "OU=Marketing,DC=lab,DC=local"
            };
            
            var validator = new RequestValidator(validOUs);

            var passwordGen = new PasswordGenerator();
            var existingUsers = new HashSet<string> {"jsmith"};
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
        }
    }
    

    
}

