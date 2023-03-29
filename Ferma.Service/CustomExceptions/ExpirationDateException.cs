using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{

    public class ExpirationDateException : Exception
    {
        public ExpirationDateException(string msg) : base(msg)
        {

        }
    }
}
