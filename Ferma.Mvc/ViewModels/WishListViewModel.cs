using Ferma.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class WishListViewModel
    {
        public List<WishlistItemViewModel> WishlistItems { get; set; }

    }
    public class WishlistItemViewModel
    {
        public Poster Poster { get; set; }
    }
}
