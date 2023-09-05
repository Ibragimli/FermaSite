using Ferma.Core.Entites;
using Ferma.Service.Helper;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace Ferma.Mvc.Areas.manage.ViewModels
{
    public class RoleManagerIndexViewModel
    {
        public PagenetedList<IdentityRole> RoleManagers { get; set; }
    }
}
