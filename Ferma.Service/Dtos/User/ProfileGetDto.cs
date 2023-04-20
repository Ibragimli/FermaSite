using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class ProfileGetDto
    {
        public AppUser User { get; set; }
        public ProfileEditDto ProfileEditDto { get; set; }
        public IEnumerable<Poster> ActivePosters { get; set; }
        public IEnumerable<Poster> DeactivePosters { get; set; }
        public IEnumerable<Poster> WaitedPosters { get; set; }
        public IEnumerable<Poster> DisabledPosters { get; set; }
        public IEnumerable<Payment> PersonalPayments { get; set; }
        public IEnumerable<Payment> PosterPayments { get; set; }
        public BalanceDto BalanceDto { get; set; }
    }
}
