using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{

    public class ItemUseException : Exception
    {
        public ItemUseException(string msg) : base(msg)
        {

        }
    }
}
