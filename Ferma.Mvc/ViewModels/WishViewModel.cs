using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class WishViewModel
    {
        public List<WishItemViewModel> WishItems { get; set; }

    }
    public class WishItemViewModel
    {
        public int PosterId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
    }
}
