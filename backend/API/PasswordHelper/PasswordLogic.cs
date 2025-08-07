
using System.Security.Cryptography;

namespace API.PasswordHelper
{
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Cryptography;
    using System.Text;

    public  class PasswordLogic
	{

        const int keySize = 64;
        const int iterations = 350000;

        public  string HashPass(string password, out byte[] salt)
		{
            var hashAlgo = HashAlgorithmName.SHA512;

            salt = SaltPass();

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, hashAlgo, keySize);

            return Convert.ToHexString(hash); 
		} 

        public  byte[] SaltPass()
        {
            return RandomNumberGenerator.GetBytes(keySize);
        }

        public  bool VerifyHashPass(string password, string hash, byte[]salt)
        {
            var hashAlgo = HashAlgorithmName.SHA512;
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgo, keySize);

            if (!CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash)))
            {
                throw new Exception("Incorrect password");
            }

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }


        public (bool success, string message, string? newHash, string? newSalt) ChangePassword(
        string newPassword, string oldPassword, string storedHash, string storedSalt)
        {
            var storedSaltBytes = Convert.FromBase64String(storedSalt);
            var isCorrect = VerifyHashPass(oldPassword, storedHash, storedSaltBytes);


            if (oldPassword == newPassword)
            {
               throw new ValidationException("Current password cannot be the same as the new password, please choose a new password");
            }

            var (newHash, newSalt) = HashAndsaltPassword(newPassword);

            return (true, "Password changed successfully", newHash, newSalt);
        }

        public (string passwordHash, string passWordSalt) HashAndsaltPassword(string password)
        {
            byte[] salt;
            var hashedPass = HashPass(password, out salt);

            var passwordHash = hashedPass;
            var passWordSalt = Convert.ToBase64String(salt);

            return (passwordHash, passWordSalt);

        }

        public string GenerateRanDomPass(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";

            var bytes = RandomNumberGenerator.GetBytes(length);

            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[bytes[i] % validChars.Length];
            }
            return new string(chars);
        }
    }
}