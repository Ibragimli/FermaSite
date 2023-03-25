using Ferma.Core.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class BalanceDto
    {
        public int Balance { get; set; }
        public string AppUserId { get; set; }
        public Source Source { get; set; }

    }
    public class BalanceDtoValidator : AbstractValidator<BalanceDto>
    {
        public BalanceDtoValidator()
        {
            RuleFor(x => x.Balance).GreaterThan(0);
        }
    }
}
