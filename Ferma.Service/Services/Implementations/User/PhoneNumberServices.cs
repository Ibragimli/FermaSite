using Ferma.Service.CustomExceptions;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ferma.Service.Services.Implementations.User
{
    public class PhoneNumberServices : IPhoneNumberServices
    {
        public void PhoneNumberValidation(string phoneNumber)
        {
            if (phoneNumber != null)
            {
                if (Regex.IsMatch(phoneNumber, "[a-zA-Z]"))
                    throw new ItemFormatException("Nömrə yanlışdır");

                if (phoneNumber.Length > 15)
                    throw new ItemFormatException("Nömrə yanlışdır");
            }
        }
        public string PhoneNumberFilter(string phoneNumber)
        {
            string number = "";
            string[] charsNumber = phoneNumber.Split('_', '-', ' ', '(', ')', ',', '.', '/', '?', '!', '+', '=', '|', '.');

            foreach (var item in charsNumber)
            {
                number += item;
            }
            return number;
        }
    }
}
