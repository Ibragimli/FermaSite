using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{

    public class UserPasswordResetException : Exception
    {
        public UserPasswordResetException(string msg) : base(msg)
        {

        }
    }
}
