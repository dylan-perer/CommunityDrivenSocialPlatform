using System.Security.Cryptography;

namespace CommunityDrivenSocialPlatform_APi.Service
{
    public class PasswordHasher
    {
        private void Hash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
