using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.CustomExceptions
{
    public class ImageFormatException : Exception
    {
        public ImageFormatException(string msg) : base(msg)
        {

        }
    }
}
