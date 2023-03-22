using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class PaymentValueException : Exception
    {
        public PaymentValueException(string msg) : base(msg)
        {

        }
    }
}
