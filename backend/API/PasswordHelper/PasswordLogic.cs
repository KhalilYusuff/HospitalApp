
using System.Security.Cryptography;

namespace API.PasswordHelper
{
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }


        public  string GenerateRanDomPass(int length)
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


        public bool PasswordValidate(string password)
        {
            return false; 
                
            
        }
	}
}