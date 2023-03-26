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
        public PagenetedList<Poster> PagenetedItemsPreVip { get; set; }
        public PagenetedList<Poster> PagenetedItemsAll { get; set; }
    }
}
