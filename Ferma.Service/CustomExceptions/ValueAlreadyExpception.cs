using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class ValueAlreadyExpception : Exception
    {
        public ValueAlreadyExpception(string msg) : base(msg)
        {

        }
    }
}
