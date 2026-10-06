using System;

namespace adtool.Models
{
    public class Results
    {
        public bool Success { get; set; }
        public string Username { get; set; }
        public string DistinguishedName { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static Results Ok(string username, string dn) => new Results
        {
            Success = true,
            Username = username,
            DistinguishedName = dn,
        };
        public static Results Fail(string em) => new Results
        {
            Success = false,
            ErrorMessage = em,
        };
    }
}