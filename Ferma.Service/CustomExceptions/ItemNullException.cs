using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
   
    public class ItemNullException : Exception
    {
        public ItemNullException(string msg) : base(msg)
        {

        }
    }
}
