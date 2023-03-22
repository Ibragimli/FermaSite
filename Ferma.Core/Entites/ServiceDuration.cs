using Ferma.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class ServiceDuration:BaseEntity
    {
        public decimal? Amount { get; set; }
        public int Duration { get; set; }
        public ServiceType ServiceType { get; set; }
    }
}
