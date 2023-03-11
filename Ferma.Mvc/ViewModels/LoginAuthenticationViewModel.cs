using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class LoginAuthenticationViewModel
    {
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
    }
}
