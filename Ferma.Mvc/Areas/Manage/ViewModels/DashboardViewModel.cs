using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.Areas.Manage.ViewModels
{
    public class DashboardViewModel
    {
        public List<Payment> Payments { get; set; }
        public List<Poster> Posters { get; set; }
        public int NewPosters { get; set; }
        public int AllPosters { get; set; }
        public int VipPosters { get; set; }
        public int PremiumPosters { get; set; }
        public int NewContact { get; set; }
        public decimal? Money { get; set; }
    }
}
