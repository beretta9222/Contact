using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Contact.Helper
{
    public class ValidationName : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value.ToString();
            if (name.Length > 50)
                return new ValidationResult(false, "ФИО не должно превышать 50 символов");
            else
                return ValidationResult.ValidResult;
        }
    }

    public class ValidationPhone : ValidationRule
    {
        private const string Pattern = @"[+]{1}\d{1}\-\d{3}\-\d{3}-\d[4]";
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string Phone = value.ToString();


            Regex regex = new Regex(Pattern);
            if (!regex.IsMatch(Phone) && Phone.Length != 15)            
                return new ValidationResult(false, "Не верный формат телефона");
            else
                return ValidationResult.ValidResult;
        }
    }
}
