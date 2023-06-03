using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class RareLimitException : Exception
    {
        public RareLimitException(string message) : base(message)
        {
        }
    }
}
