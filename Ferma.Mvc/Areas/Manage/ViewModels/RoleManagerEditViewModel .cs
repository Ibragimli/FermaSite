using Ferma.Core.Entites;
using Ferma.Service.Helper;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace Ferma.Mvc.Areas.manage.ViewModels
{
    public class RoleManagerEditViewModel
    {
        public RoleManager<IdentityRole> RoleManager { get; set; }
    }
}
