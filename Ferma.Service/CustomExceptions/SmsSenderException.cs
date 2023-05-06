using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class SmsSenderException : Exception
    {
        public SmsSenderException(string message) : base(message)
        {
        }
    }
}
