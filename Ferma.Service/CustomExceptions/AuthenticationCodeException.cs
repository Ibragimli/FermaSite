using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class AuthenticationCodeException : Exception
    {
        public AuthenticationCodeException(string msg) : base(msg)
        {

        }
    }
}
