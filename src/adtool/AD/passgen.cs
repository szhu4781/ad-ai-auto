using System.Security.Cryptography;
using System.Text;

namespace adtool.AD 
{
    public class PasswordGenerator
    {
        // No I/O and 0/1 to avoid ambiguity conflicts
        private const string Uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghjklmnpqrstuvwxyz";
        private const string Numbers = "23456789";
        private const string RegEx = "!@#$%^&*";
        private const string AllChars = Uppercase + Lowercase + Numbers + RegEx;

        /// <summary>
        /// Generates a cryptographic secure password that guarentees
        /// at least one character from each category
        /// </summary>
        public string Generate(int length = 16)
        {  
           if(length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long");
            } 

            var passwordChars = new char[length];

            // Guarentess at least one character from each category first
            passwordChars[0] = GetRandomChar(Uppercase);
            passwordChars[1] = GetRandomChar(Lowercase);
            passwordChars[2] = GetRandomChar(Numbers);
            passwordChars[3] = GetRandomChar(RegEx);

            // Fill the rest with random characters from each category
            for(int i = 4; i < length; i++)
            {
                passwordChars[i] = GetRandomChar(AllChars);
            }

            // Shuffle characters around so guarenteed characters aren't
            // always in the same starting position
            Shuffle(passwordChars);

            return new string(passwordChars);
        }

        private char GetRandomChar(string pool)
        {
            int index = RandomNumberGenerator.GetInt32(pool.Length);
            return pool[index];
        }

        private void Shuffle(char[] array)
        {
            for(int i = array.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}