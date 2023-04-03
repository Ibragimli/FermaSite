using Ferma.Core.Entites;
using Ferma.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.ViewModels
{
    public class SettingIndexViewModel
    {
        public PagenetedList<Setting> PagenatedItems { get; set; }

    }
}
