using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class ItemFormatException : Exception
    {
        public ItemFormatException(string msg) : base(msg)
        {

        }
    }
}
