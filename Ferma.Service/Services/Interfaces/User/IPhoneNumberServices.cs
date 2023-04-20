using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Services.Interfaces.User
{
    public interface IPhoneNumberServices
    {
        string PhoneNumberFilter(string phoneNumber);
        void PhoneNumberValidation(string phoneNumber);
        void PhoneNumberPrefixValidation(string phoneNumber);


    }
}
