using Ferma.Core.Entites;
using Ferma.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class IstifadeciElanViewModel
    {
        public PagenetedList<Poster> PosterAll { get; set; }
        public PagenetedList<Poster> PosterVip { get; set; }
        public PagenetedList<Poster> PosterPremium { get; set; }
    }
}
