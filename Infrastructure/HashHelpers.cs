using Infrastructure.Interfaces;
using System.Security.Cryptography;
using System.Linq;

namespace Infrastructure
{
    public class HashHelpers : IHashHelpers
    {
        public bool CompareHash(string stringToBeHashed, byte[] salt, byte[] hash)
        {
            byte[] computedHash;

            using (var hmac = new HMACSHA512(salt))
            {
                computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToBeHashed));
            }

            return computedHash.SequenceEqual(hash);
        }

        public byte[] GetNewHash(out byte[] salt, string stringToBeHashed)
        {
            byte[] hash;
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToBeHashed));
            }

            return hash;
        }
    }
}
