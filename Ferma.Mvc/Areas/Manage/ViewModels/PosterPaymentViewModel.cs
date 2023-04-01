using Ferma.Core.Entites;
using Ferma.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.ViewModels
{
    public class PosterPaymentViewModel
    {
        public PagenetedList<Payment> Payments { get; set; }

    }
}
