using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace backend.API.FieldValidator
{
    public class PasswordValidator
    {
        public bool PasswordValid(string password)
        {
          
                const int minLength = 12;
                const string validAlph = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
                const string validSpecialChar = "!@$?_-\\*";
                const string validInt = "1234567890";

                bool validLengh = password.Length > minLength;
                bool ContainsUpperChar = password.Any(c => char.IsUpper(c));
                bool ContainsMinAlph = password.Any(c => validAlph.Contains(c));
                bool ContainsSpecialChar = password.Any(c => validSpecialChar.Contains(c));
                bool ContainsInt = password.Any(c => validInt.Contains(c));


                if (validLengh && ContainsUpperChar && ContainsMinAlph && ContainsSpecialChar && ContainsInt)
                {
                    return true;
                }

                if (!validLengh)
                {
                    throw new ValidationException("Password must at least be 12 chars long");
                }
                if (!ContainsUpperChar)
                {
                    throw new ValidationException("Password must include at least 1 upper char");
                }

                if (!ContainsMinAlph)
                {
                    throw new ValidationException("Password must include at least 1 alphabet");
                }
                if (!ContainsSpecialChar)
                {
                    throw new ValidationException($"Password must include at least one of the following special characters: {validSpecialChar}");
                }

                if (!ContainsInt)
                {
                    throw new ValidationException("Password must include at least one integer value");
                }

                return false; 

        }

    }
}
