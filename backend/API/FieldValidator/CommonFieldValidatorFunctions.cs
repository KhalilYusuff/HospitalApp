using System.Text.RegularExpressions;

namespace backend.API.FieldValidator
{
    public delegate bool RequiredValidDel(string fieldval);
    public delegate bool StringLengthValDel(string fieldVal, int min, int max);
    public delegate bool DateValidDel(string dateTime, out DateTime validDateTime);
    public delegate bool PatternMatchValidDel(string fieldVal, string pattern);
    public delegate bool CompareFieldValidDel(string fieldVal, string fieldValCompare);
    public class CommonFieldValidatorFunctions
    {
        private static RequiredValidDel _requiredValidDel = null;
        private static StringLengthValDel _stringLengValDel = null;
        private static DateValidDel _dateValidDel = null;
        private static PatternMatchValidDel _fieldPatternMatchValidDel = null;
        private static CompareFieldValidDel _compareFIeldValidDel = null;

        public static RequiredValidDel RequiredFieldValidDel
        {
            get
            {
                if (_requiredValidDel == null)
                    _requiredValidDel = new RequiredValidDel(RequiredFieldValidDel);
                return _requiredValidDel;
            }
        }

        public static StringLengthValDel StringLengValDel
        {
            get
            {
                if (_stringLengValDel == null)
                    _stringLengValDel = new StringLengthValDel(StringLengValDel);
                return _stringLengValDel;
            }
        }

        public static DateValidDel DateValid
        {
            get
            {
                if (_dateValidDel == null)
                    _dateValidDel = new DateValidDel(DateValid);
                return _dateValidDel;
            }
        }

        public static PatternMatchValidDel FieldPatterMatchValid
        {
            get
            {
                if (_fieldPatternMatchValidDel == null)
                    _fieldPatternMatchValidDel = new PatternMatchValidDel(FieldPatterMatchValid);
                return _fieldPatternMatchValidDel;
            }
        }

        public static CompareFieldValidDel CompareFieldValid
        {
            get
            {
                if (_compareFIeldValidDel == null)
                    _compareFIeldValidDel = new CompareFieldValidDel(CompareFieldValid);
                return _compareFIeldValidDel;
            }
        }


        private static bool RequiredFieldValid(string fiieldVal)
        {
            if (!string.IsNullOrEmpty(fiieldVal))
            {
                return true;
            }
            return false;

        }
        private static bool StringLengthVal(string fieldVal, int min, int max)
        {
            if (fieldVal.Length >= min && fieldVal.Length <= max)
            {
                return true;
            }
            return false;
        }

        private static bool DateValidFIeld(string dateTime, out DateTime validDateTime)
        {
            if (DateTime.TryParse(dateTime, out validDateTime))
            {
                return true;
            }
            return false;
        }

        private static bool PatternMatchValidDel(string fieldVal, string regularExpressionPattern)
        {
            Regex regex = new Regex(regularExpressionPattern);
            if (regex.IsMatch(fieldVal))
            {
                return true;
            }
            return false; 
        }

        private static bool FieldCompareValid(string field1, string field2)
        {
            if (field1.Equals(field2))
            {
                return true;
            }
            return false;
        }
    }
}
