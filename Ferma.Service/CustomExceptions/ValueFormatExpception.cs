using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class ValueFormatExpception : Exception
    {
        public ValueFormatExpception(string msg) : base(msg)
        {

        }
    }
}
