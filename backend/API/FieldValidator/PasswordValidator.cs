using System.Linq;
using System.Text.RegularExpressions;

namespace backend.API.FieldValidator
{
    public class PasswordValidator
    {
        public bool PasswordValid(string password)
        {
            try
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
                    return true;

               
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;

        }

    }
}
