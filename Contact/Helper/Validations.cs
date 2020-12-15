using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
}
