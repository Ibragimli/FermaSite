using Ferma.Core.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class PaymentCreateDto
    {
        public PaymentService Services { get; set; }
        public Source Source { get; set; }
        public ServiceType ServiceType { get; set; }
        public int DurationServicesId { get; set; }
        public string AppUserId { get; set; }
        public int PosterId { get; set; }
    }
    public class PaymentCreateDtoValidator : AbstractValidator<PaymentCreateDto>
    {
        public PaymentCreateDtoValidator()
        {
            //RuleFor(x => x.).NotEmpty().WithMessage("Kategoriya hissəsi boş olmamalıdır.");
        }
    }
}
