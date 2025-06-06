﻿using Ferma.Core.Entites;
using Ferma.Core.Enums;
using Ferma.Service.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class ElanDetailViewModel
    {
        public Poster Poster { get; set; }
        public List<Poster> SimilarPosters { get; set; }
        public PosterUserId User { get; set; }
        public PaymentCreateDto PaymentCreateDto { get; set; }
        public List<ServiceDuration> ServiceDurations { get; set; }
        public int WishCount { get; set; }
    }
}
