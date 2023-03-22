using Ferma.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class Payment : BaseEntity
    {
        public PaymentService Service { get; set; }
        public Source Source { get; set; }
        public ServiceType ServiceType { get; set; }
        public decimal? Amount { get; set; }
        public int Duration { get; set; }
        public string AppUserId { get; set; }
        public int PosterId { get; set; }
        public AppUser AppUser { get; set; }
        public Poster Posters { get; set; }
    }
}
