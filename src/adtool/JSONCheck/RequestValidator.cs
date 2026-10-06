using adtool.Models;
using System.Collections.Generic;

namespace adtool.JSONCheck
{
    public class RequestValidator
    {
        private readonly HashSet<string> _validOUs;
        public RequestValidator(HashSet<string> validOUs)
        {
            _validOUs = validOUs;
        }

        /// <summary>
        /// Validates a parsed onboarding request before it's allowed
        /// to reach user/pass generation
        /// </summary>
        public ValidationResult Validate(OnboardingRequest request)
        {
            var errors = new List<string>();

            if (request == null)
            {
                errors.Add("Request was null or could not be parsed");
                return new ValidationResult(false, errors);
            }

            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                errors.Add("FirstName is required.");
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                errors.Add("LastName is required.");   
            }

            if (string.IsNullOrWhiteSpace(request.Department))
            {
                errors.Add("Department is required.");
            }

            if (string.IsNullOrWhiteSpace(request.OU))
            {
                errors.Add("OU is required.");
            } else if (!_validOUs.Contains(request.OU))
            {
                errors.Add($"OU '{request.OU}' is not a recognized organizational unit.");
            }

            return new ValidationResult(errors.Count == 0, errors);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; }
        public List<string> Errors { get; }

        public ValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }
    }
}