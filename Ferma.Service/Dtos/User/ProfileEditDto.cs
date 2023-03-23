﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class ProfileEditDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
    public class ProfileEditDtoValidator : AbstractValidator<ProfileEditDto>
    {
        public ProfileEditDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Adınızı qeyd etməlisiniz.").MinimumLength(3).WithMessage("Adınızın uzunluğu 3-dən az ola bilməz!").MaximumLength(70).WithMessage("Adınızın uzunluğu 70 dən böyük ola bilməz!");
            RuleFor(x => x.Email).MinimumLength(5).WithMessage("Emailinizin uzunluğu 5-dən az ola bilməz!").MaximumLength(70).WithMessage("Emailinizin uzunluğu 70 dən böyük ola bilməz!");
        }
    }
}
