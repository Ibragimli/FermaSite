using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
      public class ImageNullException : Exception
    {
        public ImageNullException(string msg) : base(msg)
        {

        }
    }
}
