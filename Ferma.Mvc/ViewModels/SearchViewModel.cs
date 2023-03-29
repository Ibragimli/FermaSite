using Ferma.Core.Entites;
using Ferma.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class SearchViewModel
    {
        public PagenetedList<Poster> PagenetedItemsVip { get; set; }
        public PagenetedList<Poster> PagenetedItemsPremium { get; set; }
        public PagenetedList<Poster> PagenetedItemsAll { get; set; }
    }
}
