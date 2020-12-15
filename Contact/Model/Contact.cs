using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Contact.Model
{
    public class Contact : IDataErrorInfo
    {
        private const string Pattern = @"\[+]* (?\d{3}\)?-? *\d{3}-? *-?\d{4}";

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Phone":
                        Regex regex = new Regex(Pattern);
                        if(!regex.IsMatch(Phone))
                        {
                            error = "Не верный формат телефона";
                        }
                        break;
                    case "Name":
                        if(Name.Length > 50)
                        {
                            error = "ФИО не должно превышать 50 символов";
                        }
                        break;                   
                }
                return error;
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public string Error => throw new NotImplementedException();
    }
}
