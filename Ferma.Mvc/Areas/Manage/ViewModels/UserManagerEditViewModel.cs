using Ferma.Core.Entites;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Helper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Ferma.Mvc.Areas.manage.ViewModels
{
    public class UserManagerEditViewModel
    {
        public List<IdentityRole> Roles { get; set; }
        public UserManagerEditDto UserManagerEditDto { get; set; }
        public string RoleName { get; set; }
    }
}
