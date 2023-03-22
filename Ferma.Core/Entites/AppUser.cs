using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public decimal? Balance { get; set; }
        public ICollection<PosterUserId> PosterUserIds { get; set; }
        public ICollection<WishItem> WishItems { get; set; }
        public ICollection<Payment> Payments { get; set; }

    }
}
