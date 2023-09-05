using Ferma.Core.Entites;
using Ferma.Service.Helper;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace Ferma.Mvc.Areas.manage.ViewModels
{
    public class UserManagerIndexViewModel
    {
        public PagenetedList<AppUser> AppUsers { get; set; }
    }
}
