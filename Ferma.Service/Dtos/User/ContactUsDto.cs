﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class ContactUsDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }
    public class ContactUsDtoValidator : AbstractValidator<ContactUsDto>
    {
        public ContactUsDtoValidator()
        {
            RuleFor(x => x.FullName).MaximumLength(30).WithMessage("Adınızın maksimum uzunluğu 30-ola bilər!").NotEmpty();
            RuleFor(x => x.Email).MaximumLength(70).WithMessage("Emailinizin maksimum uzunluğu 70-ola bilər!").NotEmpty();
            RuleFor(x => x.Subject).MaximumLength(70).WithMessage("Mövzunun maksimum uzunluğu 70-ola bilər!").NotEmpty();
            RuleFor(x => x.Message).MaximumLength(2500).WithMessage("Mesajınızın maksimum uzunluğu 2500-ola bilər!").NotEmpty();
        }
    }

}
