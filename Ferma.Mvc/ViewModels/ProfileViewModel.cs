using Ferma.Core.Entites;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class ProfileViewModel
    {
        public AppUser User { get; set; }
        public ProfileEditDto ProfileEditDto { get; set; }
        public List<Poster> ActivePosters { get; set; }
        public List<Poster> DeactivePosters { get; set; }
        public List<Poster> WaitedPosters { get; set; }
        public List<Poster> DisabledPosters { get; set; }
        public List<Payment> PersonalPayments { get; set; }
        public List<Payment> PosterPayments { get; set; }
        public BalanceDto BalanceDto { get; set; }
    }
}
