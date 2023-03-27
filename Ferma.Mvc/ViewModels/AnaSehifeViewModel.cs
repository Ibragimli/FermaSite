using Ferma.Core.Entites;
using Ferma.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class AnaSehifeViewModel
    {
        public PagenetedList<Poster> PagenatedItemsAll { get; set; }
        public PagenetedList<Poster> PagenatedItemsVip { get; set; }
        public List<Category> Categories { get; set; }

    }
}
