using Ferma.Core.Entites;
using Ferma.Service.Dtos.Area;
using Ferma.Service.Helper;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using System.Collections.Generic;

namespace Ferma.Mvc.Areas.manage.ViewModels
{
    public class UserManagerCreateViewModel
    {
        public List<IdentityRole> Roles { get; set; }
        public UserManagerCreateDto UserManagerCreateDto { get; set; }
    }
}
