using System.Text;

namespace adtool.AD
{
    public class UsernameGenerator
    {
        /// <summary>
        /// Checks whether a candidate username already exists.
        /// </summary>
        private readonly Func<string, bool> _usernameExists;

        public UsernameGenerator(Func<string, bool> usernameExistsCheck)
        {
            _usernameExists = usernameExistsCheck;
        }

        /// <summary>
        /// Generate a unique sAMAccountName using first initial and last initial
        /// </summary>
        public string Generate(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("First and last name are required.");
            }

            string baseName = Sanitize(firstName.Substring(0, 1) + lastName);
            string candidate = baseName;
            int suffix = 1;

            while (_usernameExists(candidate))
            {
                candidate = $"{baseName}{suffix}";
                suffix++;
            }

            return candidate;
        }

        private string Sanitize(string input)
        {
            var sb = new StringBuilder();
            foreach (char c in input.ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}